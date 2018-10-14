using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Extensions;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Dialogs
{
    public class Room : Dialog, IDialogContinue, IDialogResume
    {
        // TODO Let player actor speak these and also show them if command is known, but no actions performed
        private static readonly string[] _cannedResponses = new string[]
        {
            "You can't do that.",
            "Why?",
            "Hmm, better not.",
            "That will probably crash the game!"
        };

        private readonly Random _random;
        private readonly string _roomId;
        private readonly List<Command> _commands;

        public Room(string roomId, List<Command> commands)
        {
            _random = new Random();
            _roomId = roomId;
            _commands = commands;
        }

        public async Task DialogBegin(DialogContext dc, IDictionary<string, object> dialogArgs = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            // TODO Use RoomInitializationStarted/Completed in separate private method.

            // Send a RoomSwitched event to the client so the GUI can populate the new room.
            await dc.Context.SendActivity(CreateEventActivity(dc, "RoomSwitched", JObject.FromObject(new
            {
                roomId = _roomId
            })));

            await RunCommand(Command.RoomSwitched, dc);

            // Send a RoomEntered event to the client so the GUI can show the images for the room.
            var roomEnteredActivity = CreateEventActivity(dc, "RoomEntered");
            await dc.Context.SendActivity(roomEnteredActivity);

//            if (dc.Context.Activity.Type == ActivityTypes.Message)
  //          {
    //            await RunCommand(Command.PopulateRoom, dc);
                await RunCommand(Command.RoomEntered, dc);
      //      }
        }

        public async Task DialogContinue(DialogContext dc)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                await RunCommand(dc);
            }
        }

        public async Task DialogResume(DialogContext dc, IDictionary<string, object> result)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var state = dc.Context.GetConversationState<Dictionary<string, object>>();

            object onResumeActions = null;
            if (state.Remove("actionStack", out onResumeActions))
            {
                await ExecuteActions(dc, (List<CommandAction>)onResumeActions, true);
            }
        }

        private Task RunCommand(DialogContext dc)
        {
            return RunCommand(dc.Context.Activity.Text, dc);
        }

        private async Task RunCommand(string commandText, DialogContext dc)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var state = dc.Context.GetConversationState<Dictionary<string, object>>();

            var command = _commands
                .Where(cmd => string.Equals(cmd.Text, commandText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (command != null)
            {
                var actions = command.Actions
                    .Where(a => a.Preconditions.All(pc => state.SatifiesPrecondition(pc)));

                // Execute the actions, but don't send an Idle event yet if we've just switched rooms.
                await ExecuteActions(dc, actions, command.Text != Command.RoomSwitched);
            }
            else
            {
                // The player typed something we didn't expect; reply with a standard response.
                await ExecuteActions(dc, new CommandAction[]
                {
                    // TODO Constant?
                    new SpeakAction(_cannedResponses[_random.Next(0, _cannedResponses.Length)], "Narrator")
                }, true); 
            }
        }

        private async Task ExecuteActions(DialogContext dc, IEnumerable<CommandAction> actions, bool sendIdleEvent)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var activities = new List<IActivity>();

            var state = dc.Context.GetConversationState<Dictionary<string, object>>();
            var actionStack = new Stack<CommandAction>(actions.Reverse());

            CommandAction action;
            while (actionStack.TryPop(out action))
            {
                var nextDialogId = action.Execute(dc, activities, state);
                if (!string.IsNullOrEmpty(nextDialogId))
                {
                    if (activities.Any())
                    {
                        await dc.Context.SendActivities(activities.ToArray());
                    }

                    // Stacks don't serialize in the correct order.
                    // See https://github.com/JamesNK/Newtonsoft.Json/issues/971.
                    state["actionStack"] = actionStack.ToList();

                    // TODO We should replace when switching rooms!
                    await dc.Begin(nextDialogId);
                    return;
                }
            }

            // Add an Idle activity to let the GUI know that we're waiting for user interaction.
            if (sendIdleEvent)
            {
                activities.Add(CreateEventActivity(dc, "Idle"));
            }

            await dc.Context.SendActivities(activities.ToArray());
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