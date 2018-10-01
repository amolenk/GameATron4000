using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder.Core.Extensions;

namespace GameATron4000.Models.Actions
{
    public class SpeakAction : RoomAction
    {
        public const string Name = "Speak";

        private readonly string _text;
        private readonly string _actorName;

        public SpeakAction(string text, string actorName, IEnumerable<Precondition> preconditions = null)
            : base(preconditions != null ? preconditions.ToArray() : new Precondition[0])
        {
            _text = text;
            _actorName = actorName;
        }

        public SpeakAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _text = args[0];
            _actorName = args[1];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo) {

            activities.Add(MessageFactory.Text($"{_actorName} > {_text}"));

            return string.Empty;
        }
    }
}