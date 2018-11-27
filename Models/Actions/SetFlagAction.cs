using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class SetFlagAction : CommandAction
    {
        public const string Name = "SetFlag";

        [JsonConstructor]
        private SetFlagAction()
        {
        }

        public SetFlagAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            FlagName = args[0];
        }

        [JsonProperty]
        public string FlagName { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags) {

            flags.SetFlag(FlagName);

            return CommandActionResult.None;
        }
    }
}