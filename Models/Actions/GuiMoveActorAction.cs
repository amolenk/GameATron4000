using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiMoveActorAction : CommandAction
    {
        public const string Name = "GUI:MoveActor";

        [JsonConstructor]
        private GuiMoveActorAction()
        {
        }

        public GuiMoveActorAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ActorId = args[0];
            X = int.Parse(args[1]);
            Y = int.Parse(args[2]);
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        [JsonProperty]
        public int X { get; private set; }

        [JsonProperty]
        public int Y { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            activities.Add(CreateEventActivity(dc, "ActorMoved", JObject.FromObject(new
            {
                actorId = ActorId,
                x = X,
                y = Y
            })));

            return CommandActionResult.None;
        }
    }
}