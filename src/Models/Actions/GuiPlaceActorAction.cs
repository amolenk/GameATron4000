using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiPlaceActorAction : RoomAction
    {
        public const string Name = "GUI:PlaceActor";

        private readonly string _actorId;
        private readonly string _description;
        private readonly int _x;
        private readonly int _y;

        public GuiPlaceActorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _actorId = args[0];
            _description = args[1];
            _x = int.Parse(args[2]);
            _y = int.Parse(args[3]);
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "ActorPlacedInRoom", JObject.FromObject(new
            {
                actorId = _actorId,
                description = _description,
                x = _x,
                y = _y
            })));

            return string.Empty;
        }
    }
}