using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiPlaceObjectAction : CommandAction
    {
        public const string Name = "GUI:PlaceObject";

        [JsonConstructor]
        private GuiPlaceObjectAction()
        {
        }

        public GuiPlaceObjectAction(string objectId, Placement placement)
            : base()
        {
            ObjectId = objectId;
            Placement = placement;
        }

        public GuiPlaceObjectAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ObjectId = args[0];
            Placement = new Placement(
                int.Parse(args[1]),
                int.Parse(args[2]),
                args.Count > 3 && bool.Parse(args[3]));
        }

        [JsonProperty]
        public string ObjectId { get; private set; }

        [JsonProperty]
        public Placement Placement { get; private set; }

        [JsonProperty]
        public bool Foreground { get; private set; }
    }
}