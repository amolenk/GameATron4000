using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameATron4000.Core;
using GameATron4000.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GameATron4000.Dialogs
{
    public class ConversationDialog : Dialog
    {
        private readonly string _conversationId;
        private readonly IGameScript _script;

        public ConversationDialog(string conversationId, IGameScript script)
            : base(conversationId)
        {
            _conversationId = conversationId;
            _script = script;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var scriptResult = _script.OnConversationStarted(_conversationId);

            await ProcessScriptResultAsync(dc, scriptResult);

            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            var scriptResult = _script.OnConversationContinued(_conversationId, dc.Context.Activity.Text);

            await ProcessScriptResultAsync(dc, scriptResult);

            return new DialogTurnResult(DialogTurnStatus.Empty);
        }

        private async Task<DialogTurnResult> ProcessScriptResultAsync(DialogContext dc, IGameScriptResult scriptResult)
        {



            // Process the actions, creating a list of activities to send back to the player.
            // var activityFactory = new ActivityFactory(_gameInfo);
            // //


            if (scriptResult.ConversationOptions.Any())
            {
                // List the conversation tree options for the player.
                var options = scriptResult.ConversationOptions.Select(o => new CardAction
                    {
                        Value = o.Key,
                        DisplayText = o.Value
                    });

                // Add the conversation options to the last outbound messages activity.
                var activities = scriptResult.Activities;
                var lastMessageIndex = activities.FindLastIndex(a => a.Type == ActivityTypes.Message);
                if (lastMessageIndex > -1) 
                {
                    var lastMessageActivity = (Activity)activities[lastMessageIndex].AsMessageActivity();
                    var updatedActivity = (Activity)MessageFactory.SuggestedActions(options, lastMessageActivity.Text);
                    updatedActivity.Properties = lastMessageActivity.Properties;
                    activities[lastMessageIndex] = updatedActivity;
                }
                else
                {
                    var messageActivity = new ActivityFactory(dc.Context).LineSpoken(string.Empty, _script.World.GetSelectedActor());
                    var updatedActivity = (Activity)MessageFactory.SuggestedActions(options, messageActivity.Text); // TODO Extract
                    updatedActivity.Properties = messageActivity.Properties;

                    // TODO activities?
                    scriptResult.Activities.Add(updatedActivity);
                }

                await dc.Context.SendActivitiesAsync(scriptResult.Activities.ToArray());
            }
            else
            {
                await dc.EndDialogAsync();
            }

                        // If there are any activities, send them to the client.
            

            // // Check if the conversation tree should continue;
            // if (nextNode != null)
            // {
            //     // If there are no child nodes in the next node, there's nothing for the player to do.
            //     // Revert to the current node for the next turn.
            //     if (!nextNode.ChildNodes.Any())
            //     {
            //         nextNode = node;
            //     }

            // }

            // // Send all activities to the client.
            // await dc.Context.SendActivitiesAsync(activities.ToArray());

            // // Update the state so the next turn will start at the correct location in the dialog tree.
            // if (nextNode != null)
            // {
            //     dc.ActiveDialog.State[DialogStateCurrentNodeId] = nextNode.Id;
            //     return new DialogTurnResult(DialogTurnStatus.Waiting);
            // }

            // // Or end the dialog tree if there's no next node.
            // await dc.EndDialogAsync();

            return new DialogTurnResult(DialogTurnStatus.Complete);
        }
    }
}