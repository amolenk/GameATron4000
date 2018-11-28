using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiOpenCloseUpAction : CommandAction
    {
        public const string Name = "GUI:OpenCloseUp";

        [JsonConstructor]
        private GuiOpenCloseUpAction()
        {
        }

        public GuiOpenCloseUpAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            CloseUpId = args[0];
        }

        [JsonProperty]
        public string CloseUpId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            activities.Add(CreateEventActivity(dc, "CloseUpOpened", JObject.FromObject(new
            {
                closeUpId = CloseUpId
            })));

            return CommandActionResult.None;
        }
    }
}