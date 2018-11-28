using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GoToConversationTopicAction : CommandAction
    {
        public const string Name = "GoToConversationTopic";

        [JsonConstructor]
        private GoToConversationTopicAction()
        {
        }

        public GoToConversationTopicAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            Topic = args[0];
        }

        [JsonProperty]
        public string Topic { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags)
        {
            return CommandActionResult.None;
        }
    }
}