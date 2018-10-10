using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class EndConversationAction : RoomAction
    {
        public const string Name = "EndConversation";

        public EndConversationAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state)
        {
            return string.Empty;
        }
    }
}