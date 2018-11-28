using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class ClearFlagAction : CommandAction
    {
        public const string Name = "ClearFlag";

        [JsonConstructor]
        private ClearFlagAction()
        {
        }

        public ClearFlagAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            FlagName = args[0];
        }

        [JsonProperty]
        public string FlagName { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags)
        {
            flags.ClearFlag(FlagName);

            return CommandActionResult.None;
        }
    }
}