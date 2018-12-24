using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class EndConversationAction : CommandAction
    {
        public const string Name = "EndConversation";

        [JsonConstructor]
        private EndConversationAction()
        {
        }

        public EndConversationAction(List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
        }
    }
}