using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiMoveActorAction : CommandAction
    {
        public const string Name = "GUI:MoveActor";

        [JsonConstructor]
        private GuiMoveActorAction()
        {
        }

        public GuiMoveActorAction(string actorId, ActorPosition position, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            ActorId = actorId;
            Position = position;
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        [JsonProperty]
        public ActorPosition Position { get; private set; }
    }
}