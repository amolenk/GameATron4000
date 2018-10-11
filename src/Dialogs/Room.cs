using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // TODO No room definition blob needed anymore???
            // Send a RoomEntered event to the client so the GUI can show the images for the room.
            var roomEnteredActivity = CreateEventActivity(dc, "RoomEntered", JObject.FromObject(new
            {
                roomId = _roomId
            }));
            await dc.Context.SendActivity(roomEnteredActivity);

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                await RunCommand(Command.RoomEntered, dc);
            }
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
                await ExecuteActions(dc, (List<RoomAction>)onResumeActions);
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

            // Function to check if the dialog state contains the given flag.
            Func<Precondition, bool> verifyPrecondition = new Func<Precondition, bool>(pc =>
            {
                var key = "flag_" + pc.Flag;
                return pc.Value ? state.ContainsKey(key) : !state.ContainsKey(key);
            });

            var command = _commands
                .Where(cmd => string.Equals(cmd.Text, commandText, StringComparison.OrdinalIgnoreCase)
                    && cmd.Preconditions.All(verifyPrecondition))
                .FirstOrDefault();

            if (command != null)
            {
                var actions = command.Actions.Where(a => a.Preconditions.All(verifyPrecondition));

                await ExecuteActions(dc, actions);
            }
            else
            {
                // The player typed something we didn't expect; reply with a standard response.
                await ExecuteActions(dc, new RoomAction[]
                {
                    // TODO Constant?
                    new SpeakAction(_cannedResponses[_random.Next(0, _cannedResponses.Length)], "Narrator")
                }); 
            }
        }

        private async Task ExecuteActions(DialogContext dc, IEnumerable<RoomAction> actions)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var activities = new List<IActivity>();
            // var updatedFlags = new Dictionary<string, object>();

            var state = dc.Context.GetConversationState<Dictionary<string, object>>();
            var actionStack = new Stack<RoomAction>(actions.Reverse());

            RoomAction action;
            while (actionStack.TryPop(out action))
            {
                var nextDialogId = action.Execute(dc, activities, state);
                if (!string.IsNullOrEmpty(nextDialogId))
                {
                    if (activities.Any())
                    {
                        await dc.Context.SendActivities(activities.ToArray());
                    }

                    // TODO Make sure we don't loose the updated flags!!!

                    // Stacks don't serialize in the correct order.
                    // See https://github.com/JamesNK/Newtonsoft.Json/issues/971.
                    state["actionStack"] = actionStack.ToList();

                    await dc.Begin(nextDialogId);
                    return;
                }
            }

            // Commit the temporary updated flags dictionary to the actual state object.
            // UpdateState(state, updatedFlags);

            // Add an Idle activity to let the GUI know that we're waiting for user interaction.
            // TODO Use action for this???
            activities.Add(CreateEventActivity(dc, "Idle"));

            await dc.Context.SendActivities(activities.ToArray());
        }

        // private void UpdateState(Dictionary<string, object> state, Dictionary<string, object> updatedFlags)
        // {
        //     foreach (var updatedFlag in updatedFlags)
        //     {
        //         var key = "flag_" + updatedFlag.Key;
        //         var flagIsSet = state.ContainsKey(key);

        //         if (flagIsSet && !((bool)updatedFlag.Value))
        //         {
        //             state.Remove(key);
        //         }
        //         else if (!flagIsSet && ((bool)updatedFlag.Value))
        //         {
        //             state.Add(key, true);
        //         }
        //     }
        // }

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