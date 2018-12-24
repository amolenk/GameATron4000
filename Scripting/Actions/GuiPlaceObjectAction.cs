using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiPlaceObjectAction : CommandAction
    {
        public const string Name = "GUI:PlaceObject";

        [JsonConstructor]
        private GuiPlaceObjectAction()
        {
        }

        public GuiPlaceObjectAction(string objectId, GamePosition position, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            ObjectId = objectId;
            Position = position;
        }

        [JsonProperty]
        public string ObjectId { get; private set; }

        [JsonProperty]
        public GamePosition Position { get; private set; }
    }
}