using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace GameATron4000.Models.Actions
{
    public class StartConversationAction : RoomAction
    {
        public const string Name = "StartConversation";

        private readonly string _conversationId;

        public StartConversationAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _conversationId = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state)
        {
            return _conversationId;
        }
    }
}