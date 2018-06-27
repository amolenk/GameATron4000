using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

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
        private readonly List<Command> _commands;

        public Room(List<Command> commands)
        {
            _random = new Random();
            _commands = commands;
        }

        public Task DialogBegin(DialogContext dc, IDictionary<string, object> dialogArgs = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            return RunCommand(dc, dialogArgs);
        }

        public async Task DialogContinue(DialogContext dc)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                await RunCommand(dc, new Dictionary<string, object> { { "Activity", dc.Context.Activity } });
            }
        }

        public Task DialogResume(DialogContext dc, IDictionary<string, object> result)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            return RunCommand(dc, result);
        }

        private async Task RunCommand(DialogContext dc, IDictionary<string, object> result = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var state = dc.Context.GetConversationState<Dictionary<string, object>>();

            // Function to check if the dialog state contains the given flag.
            Func<Precondition, bool> verifyPrecondition = new Func<Precondition, bool>(pc =>
            {
                var key = "flag_" + pc.Flag;
                return pc.Value ? state.ContainsKey(key) : !state.ContainsKey(key);
            });

            var lowerCaseCommand = dc.Context.Activity.Text.ToLowerInvariant();
            var command = _commands
                .Where(cmd => cmd.Text == lowerCaseCommand && cmd.Preconditions.All(verifyPrecondition))
                .FirstOrDefault();

            var activities = new List<IActivity>();
            var updatedFlags = new Dictionary<string, bool>();

            if (command != null)
            {
                foreach (var action in command.Actions.Where(a => a.Preconditions.All(verifyPrecondition)))
                {
                    switch (action.Name)
                    {
                        case Models.Action.AddToInventory:
                            // Don't update the state directly, because the behaviour of other actions may
                            // still depend on the original state.
                            updatedFlags[action.Args[0]] = true;
                            break;

                        case Models.Action.Describe:
                            activities.Add(MessageFactory.Text(action.Args[0]));
                            break;

                        case Models.Action.Speak:
                            activities.Add(MessageFactory.Text($"{action.Args[1]} > {action.Args[0]}"));
                            break;
                    }
                }
            }

            // If there's nothing to return to the player, reply with a standard response.
            if (!activities.Any())
            {
                activities.Add(MessageFactory.Text("Narrator > " + _cannedResponses[_random.Next(0, _cannedResponses.Length)]));
            }

            // Commit the temporary updated flags dictionary to the actual state object.
            UpdateState(state, updatedFlags);

            await dc.Context.SendActivities(activities.ToArray());
        }

        private void UpdateState(Dictionary<string, object> state, Dictionary<string, bool> updatedFlags)
        {
            foreach (var updatedFlag in updatedFlags)
            {
                var key = "flag_" + updatedFlag.Key;
                var flagIsSet = state.ContainsKey(key);

                if (flagIsSet && !updatedFlag.Value)
                {
                    state.Remove(key);
                }
                else if (!flagIsSet && updatedFlag.Value)
                {
                    state.Add(key, true);
                }
            }
        }
    }
}