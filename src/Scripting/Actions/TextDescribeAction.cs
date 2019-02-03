using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder;

namespace GameATron4000.Scripting.Actions
{
    public class TextDescribeAction : CommandAction
    {
        public const string Name = "Text:Describe";

        [JsonConstructor]
        private TextDescribeAction()
        {
        }

        public TextDescribeAction(string text, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Text = text;
        }

        [JsonProperty]
        public string Text { get; private set; }
    }
}