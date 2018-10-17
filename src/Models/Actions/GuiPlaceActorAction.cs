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

        public GuiPlaceActorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ActorId = args[0];
            Description = args[1];
            X = int.Parse(args[2]);
            Y = int.Parse(args[3]);

            if (args.Count > 4)
            {
                TextColor = args[4];
            }
            else
            {
                TextColor = "White";
            }
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public int X { get; private set; }

        [JsonProperty]
        public int Y { get; private set; }

        [JsonProperty]
        public string TextColor { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "ActorPlacedInRoom", JObject.FromObject(new
            {
                actorId = ActorId,
                description = Description,
                x = X,
                y = Y,
                textColor = TextColor
            })));

            return CommandActionResult.None;
        }
    }
}