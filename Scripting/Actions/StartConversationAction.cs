using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace GameATron4000.Scripting.Actions
{
    public class StartConversationAction : CommandAction
    {
        public const string Name = "StartConversation";

        [JsonConstructor]
        private StartConversationAction()
        {
        }

        public StartConversationAction(string conversationId, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            ConversationId = conversationId;
        }

        [JsonProperty]
        public string ConversationId { get; private set; }
    }
}