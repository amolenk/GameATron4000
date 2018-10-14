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

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            activities.Add(CreateEventActivity(dc, "ActorFacedFront", JObject.FromObject(new
            {
                actorId = ActorId
            })));

            return string.Empty;
        }
    }
}