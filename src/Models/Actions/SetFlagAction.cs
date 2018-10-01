using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class SetFlagAction : RoomAction
    {
        public const string Name = "SetFlag";

        private readonly string _flagName;

        public SetFlagAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _flagName = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo) {

            state["flag_" + _flagName] = true;

            return string.Empty;
        }
    }
}