using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiPlaceActorAction : CommandAction
    {
        public const string Name = "GUI:PlaceActor";

        [JsonConstructor]
        private GuiPlaceActorAction()
        {
        }

        public GuiPlaceActorAction(string actorId, Placement placement)
            : base()
        {
            ActorId = actorId;
            Placement = placement;
        }

        public GuiPlaceActorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ActorId = args[0];
            Placement = new Placement(
                int.Parse(args[1]),
                int.Parse(args[2]));
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        [JsonProperty]
        public Placement Placement { get; private set; }
    }
}