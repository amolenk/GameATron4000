using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class SwitchRoomAction : CommandAction
    {
        public const string Name = "SwitchRoom";

        [JsonConstructor]
        private SwitchRoomAction()
        {
        }

        public SwitchRoomAction(string roomId, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            RoomId = roomId;
        }

        [JsonProperty]
        public string RoomId { get; private set; }
    }
}