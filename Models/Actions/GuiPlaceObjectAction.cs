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

        public GuiPlaceObjectAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ObjectId = args[0];
            Description = args[1];
            X = int.Parse(args[2]);
            Y = int.Parse(args[3]);

            if (args.Count > 4)
            {
                Foreground = bool.Parse(args[4]);
            }
        }

        [JsonProperty]
        public string ObjectId { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public int X { get; private set; }

        [JsonProperty]
        public int Y { get; private set; }

        [JsonProperty]
        public bool Foreground { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            activities.Add(CreateEventActivity(dc, "ObjectPlacedInRoom", JObject.FromObject(new
            {
                objectId = ObjectId,
                description = Description,
                x = X,
                y = Y,
                foreground = Foreground
            })));

            return CommandActionResult.None;
        }
    }
}