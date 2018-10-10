using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiDelayAction : RoomAction
    {
        public const string Name = "GUI:Delay";

        private readonly int _milliseconds;

        public GuiDelayAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _milliseconds = int.Parse(args[0]);
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "Delayed", JObject.FromObject(new
            {
                time = _milliseconds
            })));

            return string.Empty;
        }
    }
}