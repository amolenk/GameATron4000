using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    // TODO Make all action classes serializable!!!
    public class GuiFaceActorFrontAction : CommandAction
    {
        public const string Name = "GUI:FaceActorFront";

        [JsonConstructor]
        private GuiFaceActorFrontAction()
        {
        }

        public GuiFaceActorFrontAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ActorId = args[0];
        }

        [JsonProperty]
        public string ActorId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            activities.Add(CreateEventActivity(dc, "ActorFacedFront", JObject.FromObject(new
            {
                actorId = ActorId
            })));

            return CommandActionResult.None;
        }
    }
}