using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiChangeActorDirectionAction : CommandAction
    {
        public const string Name = "GUI:ChangeActorDirection";

        [JsonConstructor]
        private GuiChangeActorDirectionAction()
        {
        }

        public GuiChangeActorDirectionAction(string actorId, ActorDirection direction,
            List<ActionPrecondition> preconditions) : base(preconditions)
        {
            ActorId = actorId;
            Direction = direction;
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        [JsonProperty]
        public ActorDirection Direction { get; private set; }
    }
}