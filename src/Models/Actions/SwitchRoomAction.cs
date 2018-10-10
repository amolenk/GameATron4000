using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class SwitchRoomAction : RoomAction
    {
        public const string Name = "SwitchRoom";

        private readonly string _roomId;

        public SwitchRoomAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _roomId = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            return _roomId;
        }
    }
}