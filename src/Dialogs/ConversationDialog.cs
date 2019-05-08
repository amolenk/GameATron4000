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
            if (scriptResult.NextDialogId.Length > 0)
            {
                if (scriptResult.Activities.Any())
                {
                    await dc.Context.SendActivitiesAsync(scriptResult.Activities.ToArray());
                }

                await dc.EndDialogAsync(scriptResult);
            }
            else
            {
                var endDialog = true;

                if (scriptResult.ConversationOptions.Any())
                {
                    // List the conversation tree options for the player.
                    var options = scriptResult.ConversationOptions.Select(o => new CardAction
                        {
                            Value = o.Key,
                            DisplayText = o.Value
                        });

                    // Add a new outbound message activity containing the options.
                    // var activities = scriptResult.Activities;
                    // var lastMessageIndex = activities.FindLastIndex(a => a.Type == ActivityTypes.Message);
                    // if (lastMessageIndex > -1) 
                    // {
                    //     var lastMessageActivity = (Activity)activities[lastMessageIndex].AsMessageActivity();
                    //     var updatedActivity = (Activity)MessageFactory.SuggestedActions(options, lastMessageActivity.Text);
                    //     updatedActivity.Properties = lastMessageActivity.Properties;
                    //     activities[lastMessageIndex] = updatedActivity;
                    // }
                    // else
                    // {
                        var messageActivity = new ActivityFactory(dc.Context).LineSpoken(string.Empty, _script.World.GetSelectedActor());
                        var updatedActivity = (Activity)MessageFactory.SuggestedActions(options, messageActivity.Text); // TODO Extract
                        updatedActivity.Properties = messageActivity.Properties;

                        scriptResult.Activities.Add(updatedActivity);
                    //}
                    endDialog = false;
                }

                if (scriptResult.Activities.Any())
                {
                    await dc.Context.SendActivitiesAsync(scriptResult.Activities.ToArray());
                }

                if (endDialog)
                {
                    await dc.EndDialogAsync();
                }
            }

            return new DialogTurnResult(DialogTurnStatus.Complete);
        }
    }
}