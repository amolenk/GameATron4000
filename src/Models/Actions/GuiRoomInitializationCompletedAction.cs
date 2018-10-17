
using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiRoomInitializationCompletedAction : CommandAction
    {
        public GuiRoomInitializationCompletedAction()
        {
        }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state)
        {
            activities.Add(CreateEventActivity(dc, "RoomInitializationCompleted"));

            return CommandActionResult.None;
        }
    }
}