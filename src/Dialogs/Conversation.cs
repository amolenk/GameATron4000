using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace GameATron4000.Dialogs
{
    public class Conversation : Dialog, IDialogContinue
    {
        public Conversation()
        {
        }

        public Task DialogBegin(DialogContext dc, IDictionary<string, object> dialogArgs = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            return RunStep(dc, dialogArgs);
        }

        public async Task DialogContinue(DialogContext dc)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (dc.Context.Activity.Type == ActivityTypes.Message)
            {
                await RunStep(dc, new Dictionary<string, object> { { "Activity", dc.Context.Activity } });
            }
        }
        private async Task RunStep(DialogContext dc, IDictionary<string, object> result = null)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (dc.Context.Activity.AsMessageActivity().Text == "bye")
            {
                await dc.End();
            }
            else
            {
                await dc.Context.SendActivity("chirp-chirp");

                var state = dc.Context.GetConversationState<Dictionary<string, object>>();

                await dc.Context.SendActivity(string.Join(", ", state.Keys));
            }   
        }
    }
}