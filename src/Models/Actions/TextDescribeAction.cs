using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder.Core.Extensions;

namespace GameATron4000.Models.Actions
{
    public class TextDescribeAction : RoomAction
    {
        public const string Name = "Text:Describe";

        private readonly string _text;

        public TextDescribeAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _text = args[0];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo) {

            activities.Add(MessageFactory.Text(_text));

            return string.Empty;
        }
    }
}