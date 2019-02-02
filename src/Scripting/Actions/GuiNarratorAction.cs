using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiNarratorAction : CommandAction
    {
        public const string Name = "GUI:Narrator";

        [JsonConstructor]
        private GuiNarratorAction()
        {
        }

        public GuiNarratorAction(string text, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Text = text;
        }

        [JsonProperty]
        public string Text { get; private set; }
    }
}