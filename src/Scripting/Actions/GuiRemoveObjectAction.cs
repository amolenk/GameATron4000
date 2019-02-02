using System.Collections.Generic;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GameATron4000.Scripting.Actions
{
    public class GuiRemoveObjectAction : CommandAction
    {
        public const string Name = "GUI:RemoveObject";

        [JsonConstructor]
        private GuiRemoveObjectAction()
        {
        }

        public GuiRemoveObjectAction(string objectId, List<ActionPrecondition> preconditions)
            : base(preconditions)
        {
            ObjectId = objectId;
        }

        [JsonProperty]
        public string ObjectId { get; private set; }
    }
}