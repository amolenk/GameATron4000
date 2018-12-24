using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiFaceActorFrontAction : CommandAction
    {
        public const string Name = "GUI:FaceActorFront";

        [JsonConstructor]
        private GuiFaceActorFrontAction()
        {
        }

        public GuiFaceActorFrontAction(string actorId, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            ActorId = actorId;
        }

        [JsonProperty]
        public string ActorId { get; private set; }
    }
}