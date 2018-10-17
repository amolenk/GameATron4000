using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Models;
using GameATron4000.Models.Actions;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GameATron4000.Dialogs
{
    public class Conversation : Dialog, IDialogContinue
    {
        private readonly ConversationNode _rootNode;

        public Conversation(ConversationNode rootNode)
        {
            _rootNode = rootNode;
        }

        public Task DialogBegin(DialogContext dc, IDictionary<string, object> dialogArgs = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            return RunStep(dc);
        }

        public async Task DialogContinue(DialogContext dc)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                await RunStep(dc, dc.Context.Activity.Text);
            }
        }

        private async Task RunStep(DialogContext dc, string option = null)
        {
            var state = dc.Context.GetConversationState<Dictionary<string, object>>();

            // Find the current conversation tree node using the saved Step state.
            var node = dc.ActiveDialog.Step == 0 ? _rootNode : _rootNode.Find(dc.ActiveDialog.Step);

            // Find the node that contains the actions for the reply.
            var nextNode = (option != null && node.ChildNodes.ContainsKey(option))
                ? node.ChildNodes[option]
                : node;

            // Process the actions, creating a list of activities to send back to the player.
            var activities = new List<IActivity>();
            //
            foreach (var action in nextNode.Actions)
            {
                switch (action)
                {
                    case GoToConversationTopicAction goToConversationTopicAction:
                        if (string.Equals(goToConversationTopicAction.Topic, "root", StringComparison.OrdinalIgnoreCase))
                        {
                            nextNode = _rootNode;
                        }
                        else if (node.ParentId.HasValue)
                        {
                            nextNode = _rootNode.Find(node.ParentId.Value);
                        }
                        break;

                    case EndConversationAction endConversationAction:
                        nextNode = null;
                        break;

                    default:
                        action.Execute(dc, activities, state);
                        break;
                }
            }

            // Check if the conversation tree should continue;
            if (nextNode != null)
            {
                // If there are no child nodes in the next node, there's nothing for the player to do.
                // Revert to the current node for the next turn.
                if (!nextNode.ChildNodes.Any())
                {
                    nextNode = node;
                }

                // List the conversation tree options for the player.
                var options = nextNode.ChildNodes.Select(s => s.Key).ToArray();

                // Add the conversation tree options to the last outbound messages activity.
                var lastMessageIndex = activities.FindLastIndex(a => a.Type == ActivityTypes.Message);
                var text = activities[lastMessageIndex].AsMessageActivity().Text;
                activities[lastMessageIndex] = MessageFactory.SuggestedActions(options, text);
            }

            // Send all activities to the client.
            await dc.Context.SendActivities(activities.ToArray());

            // Update the state so the next turn will start at the correct location in the dialog tree.
            if (nextNode != null)
            {
                dc.ActiveDialog.Step = nextNode.Id;
            }
            // Or end the dialog tree if there's no next node.
            else
            {
                await dc.End();
            }
        }
    }
}