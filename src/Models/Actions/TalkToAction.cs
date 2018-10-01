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
    public class TalkToAction : RoomAction
    {
        public const string Name = "TalkTo";

        private readonly string _conversationId;

        public TalkToAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _conversationId = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo)
        {
            return _conversationId;
        }
    }
}