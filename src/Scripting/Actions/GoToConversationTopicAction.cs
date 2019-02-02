using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GoToConversationTopicAction : CommandAction
    {
        public const string Name = "GoToConversationTopic";

        [JsonConstructor]
        private GoToConversationTopicAction()
        {
        }

        public GoToConversationTopicAction(string topic, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Topic = topic;
        }

        [JsonProperty]
        public string Topic { get; private set; }
    }
}