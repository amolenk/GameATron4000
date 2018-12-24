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
    public class SpeakAction : CommandAction
    {
        public const string Name = "Speak";

        [JsonConstructor]
        private SpeakAction()
        {
        }
        
        public SpeakAction(string text, string actorId, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Text = text;
            ActorId = actorId;
        }

        [JsonProperty]
        public string Text { get; private set; }

        [JsonProperty]
        public string ActorId { get; private set; }
    }
}