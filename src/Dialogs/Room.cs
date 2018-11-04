using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Extensions;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Dialogs
{
    public class Room : Dialog
    {
        private readonly string _roomId;
        private readonly List<Command> _commands;
        private readonly string[] _badCommandResponses;
        private readonly string _playerActorId;
        private readonly Random _random;
        private readonly GameBotAccessors _stateAccessors;

        public Room(string roomId, List<Command> commands, string[] badCommandResponses, string playerActorId,
            GameBotAccessors stateAccessors) : base(roomId)
        {
            _roomId = roomId;
            _commands = commands;
            _badCommandResponses = badCommandResponses;
            _playerActorId = playerActorId;
            _random = new Random();
            _stateAccessors = stateAccessors;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var state = await _stateAccessors.GameState.GetAsync(dc.Context, 
                () => new Dictionary<string,object>(), cancellationToken);

            // When the player enters a new room, send a bunch of activities to initialize the room.
            var actions = new List<CommandAction>();

            // Room initialization takes care of populating the room with actors and objects.
            actions.Add(new GuiRoomInitializationStartedAction(_roomId));
            actions.AddRange(GetActions(Command.InitializeRoom, state));
            actions.Add(new GuiRoomInitializationCompletedAction());

            // There may be some actions that must be performed immediately after the player
            // enters the room (e.g. walk to a specific spot). Add those actions to the list as well.
            actions.AddRange(GetActions(Command.EnterRoom, state));

            // Map the actions to Bot Framework activities and send them to the client.
            await ExecuteActions(dc, actions, state);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // The activity text contains the player command.
            var command = dc.Context.Activity.Text;
            var state = await _stateAccessors.GameState.GetAsync(dc.Context, 
                () => new Dictionary<string,object>(), cancellationToken);

            // Get the actions for the command from the game script.
            var actions = GetActions(command, state);

            // If there are no actions; reply with a standard response.
            if (!actions.Any())
            {
                actions = new CommandAction[]
                {
                    new SpeakAction(_badCommandResponses[_random.Next(0, _badCommandResponses.Length)], _playerActorId)
                };
            }

            // Map the actions to Bot Framework activities and send them to the client.
            await ExecuteActions(dc, actions, state);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dc, DialogReason reason, object result = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var state = await _stateAccessors.GameState.GetAsync(dc.Context, 
                () => new Dictionary<string,object>(), cancellationToken);

            // We've just finished a conversation with an actor. Check the state if the game script
            // contains any further actions to execute.
            object onResumeActions = null;
            if (state.Remove("actionStack", out onResumeActions))
            {
                // Map the actions to Bot Framework activities and send them to the client.
                await ExecuteActions(dc, (List<CommandAction>)onResumeActions, state);
            }

            // save state
            await _stateAccessors.GameState.SetAsync(dc.Context, state);
            await _stateAccessors.ConversationState.SaveChangesAsync(dc.Context, true, cancellationToken);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private IEnumerable<CommandAction> GetActions(string commandText, IDictionary<string, object> state)
        {
            // Try to find a matching command for the player's input.
            var command = _commands
                .Where(cmd => string.Equals(cmd.Text, commandText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            // If we've found a matching command, return all actions for which the preconditions can be
            // satisfied.
            return (command != null)
                ? command.Actions.Where(a => a.Preconditions.All(pc => state.SatifiesPrecondition(pc)))
                : Enumerable.Empty<CommandAction>();
        }

        private async Task ExecuteActions(DialogContext dc, IEnumerable<CommandAction> actions, IDictionary<string, object> state)
        {
            var activities = new List<IActivity>();
            var actionStack = new Stack<CommandAction>(actions.Reverse());

            // Process each action in turn, populating the activities list.
            CommandAction action;
            while (actionStack.TryPop(out action))
            {
                var nextDialogAction = action.Execute(dc, activities, state);
                if (nextDialogAction.NextDialogType != DialogType.None)
                {
                    // If the result of the action indicates that we need to start a new dialog
                    // (e.g. conversation with an actor, or a switch to a new room), first send all
                    // activities collected up to this point to the client.
                    if (activities.Any())
                    {
                        await dc.Context.SendActivitiesAsync(activities.ToArray());
                    }

                    if (nextDialogAction.NextDialogType == DialogType.Conversation)
                    {
                        // If the player starts a conversation, save any remaining actions to the state.
                        // These will be executed when the conversation is done and this dialog
                        // continues.
                        if (actionStack.Any())
                        {
                            // Conver to List because Stacks don't serialize in the correct order.
                            // See https://github.com/JamesNK/Newtonsoft.Json/issues/971.
                            state["actionStack"] = actionStack.ToList();
                        }

                        // Start the conversation.
                        await dc.BeginDialogAsync(nextDialogAction.NextDialogId);
                    }
                    else
                    {
                        // Switch to the new room.
                        await dc.ReplaceDialogAsync(nextDialogAction.NextDialogId);
                    }

                    // Stop processing any further actions now that we've switched to a new dialog.
                    return;
                }
            }

            // Add an Idle activity to let the GUI know that we're waiting for user interaction.
            activities.Add(CreateEventActivity(dc, "Idle"));

            await dc.Context.SendActivitiesAsync(activities.ToArray());
        }

        private static Activity CreateEventActivity(DialogContext dc, string name, JObject properties = null)
        {
            var eventActivity = dc.Context.Activity.CreateReply();
            eventActivity.Type = "event";
            eventActivity.Name = name;
            eventActivity.Properties = properties;
            return eventActivity;
        }
    }
}