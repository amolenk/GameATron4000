using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiMoveActorAction : RoomAction
    {
        public const string Name = "GUI:MoveActor";

        private readonly string _actorName;
        private readonly int _x;
        private readonly int _y;

        public GuiMoveActorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _actorName = args[0];
            _x = int.Parse(args[1]);
            _y = int.Parse(args[2]);
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state, GameRoom roomInfo) {

            activities.Add(CreateEventActivity(dc, "ActorMoved", JObject.FromObject(new
            {
                actorId = _actorName,
                x = _x,
                y = _y
            })));

            return string.Empty;
        }
    }
}