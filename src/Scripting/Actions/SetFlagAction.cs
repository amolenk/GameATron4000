using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class SetFlagAction : CommandAction
    {
        public const string Name = "SetFlag";

        [JsonConstructor]
        private SetFlagAction()
        {
        }

        public SetFlagAction(string flag, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            Flag = flag;
        }

        [JsonProperty]
        public string Flag { get; private set; }
    }
}