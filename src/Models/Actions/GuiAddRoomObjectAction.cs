using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiAddRoomObjectAction : RoomAction
    {
        public const string Name = "GuiAddRoomObject";

        private readonly string _objectId;
        private readonly int _x;
        private readonly int _y;
        private readonly bool _foreground;

        public GuiAddRoomObjectAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _objectId = args[0];
            _x = int.Parse(args[1]);
            _y = int.Parse(args[2]);
            _foreground = args.Count > 3 ? bool.Parse(args[3]) : false;
        }

        // TODO Can't we simply cache the room definition on the client??
        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo) {

            activities.Add(CreateEventActivity(dc, "RoomObjectAdded", JObject.FromObject(new
            {
                objectId = _objectId,
                objectDescription = roomInfo.Objects.First(o => o.Id == _objectId).Description,
                _x = _x,
                _y = _y,
                _foreground = _foreground
            })));

            return string.Empty;
        }
    }
}