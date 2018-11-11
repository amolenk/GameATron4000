using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class RemoveFromInventoryAction : CommandAction
    {
        public const string Name = "RemoveFromInventory";

        [JsonConstructor]
        private RemoveFromInventoryAction()
        {
        }

        public RemoveFromInventoryAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            InventoryItemId = args[0];
        }

        [JsonProperty]
        public string InventoryItemId { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags)
        {
            flags.ClearFlag(InventoryItemId);
            
            activities.Add(CreateEventActivity(dc, "InventoryItemRemoved", JObject.FromObject(new
            {
                inventoryItemId = InventoryItemId
            })));

            return CommandActionResult.None;
        }
    }
}