using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiDelayAction : CommandAction
    {
        public const string Name = "GUI:Delay";

        [JsonConstructor]
        private GuiDelayAction()
        {
        }

        public GuiDelayAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            Milliseconds = int.Parse(args[0]);
        }

        [JsonProperty]
        public int Milliseconds { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "Delayed", JObject.FromObject(new
            {
                time = Milliseconds
            })));

            return CommandActionResult.None;
        }
    }
}