using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder;

namespace GameATron4000.Models.Actions
{
    public class SpeakAction : CommandAction
    {
        public const string Name = "Speak";

        [JsonConstructor]
        private SpeakAction()
        {
        }

        public SpeakAction(string text, string actorId, IEnumerable<Precondition> preconditions = null)
            : base(preconditions != null ? preconditions.ToArray() : new Precondition[0])
        {
            Text = text;
            ActorId = actorId;
        }

        public SpeakAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            Text = args[0];
            ActorId = args[1];
        }

        [JsonProperty]
        public string Text { get; private set; }

        [JsonProperty]
        public string ActorId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            activities.Add(MessageFactory.Text($"{ActorId} > {Text}"));

            return CommandActionResult.None;
        }
    }
}