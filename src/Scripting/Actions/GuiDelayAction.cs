using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiDelayAction : CommandAction
    {
        public const string Name = "GUI:Delay";

        [JsonConstructor]
        private GuiDelayAction()
        {
        }

        public GuiDelayAction(int milliseconds, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Milliseconds = milliseconds;
        }

        [JsonProperty]
        public int Milliseconds { get; private set; }
    }
}