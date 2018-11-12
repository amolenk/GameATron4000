using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class SwitchRoomAction : CommandAction
    {
        public const string Name = "SwitchRoom";

        [JsonConstructor]
        private SwitchRoomAction()
        {
        }

        public SwitchRoomAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            RoomId = args[0];
        }

        [JsonProperty]
        public string RoomId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            return CommandActionResult.SwitchRoom(RoomId);
        }
    }
}