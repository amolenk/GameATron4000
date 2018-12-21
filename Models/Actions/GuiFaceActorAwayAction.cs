using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Models.Actions
{
    public class GuiFaceActorAwayAction : CommandAction
    {
        public const string Name = "GUI:FaceActorAway";

        [JsonConstructor]
        private GuiFaceActorAwayAction()
        {
        }

        public GuiFaceActorAwayAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            ActorId = args[0];
        }

        [JsonProperty]
        public string ActorId { get; private set; }
    }
}