using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder.Core.Extensions;

namespace GameATron4000.Models.Actions
{
    public class GuiNarratorAction : CommandAction
    {
        public const string Name = "GUI:Narrator";

        [JsonConstructor]
        private GuiNarratorAction()
        {
        }

        public GuiNarratorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            Text = args[0];
        }

        [JsonProperty]
        public string Text { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "Narrated", JObject.FromObject(new
            {
                text = Text
            })));

            return CommandActionResult.None;
        }
    }
}