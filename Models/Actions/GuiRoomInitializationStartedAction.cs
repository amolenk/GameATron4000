
using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiRoomInitializationStartedAction : CommandAction
    {
        [JsonConstructor]
        private GuiRoomInitializationStartedAction()
        {
        }

        public GuiRoomInitializationStartedAction(string roomId)
        {
            RoomId = roomId;
        }

        [JsonProperty]
        public string RoomId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags)
        {
            activities.Add(CreateEventActivity(dc, "RoomInitializationStarted", JObject.FromObject(new
            {
                roomId = RoomId
            })));

            return CommandActionResult.None;
        }
    }
}