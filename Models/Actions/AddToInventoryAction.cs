using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class AddToInventoryAction : CommandAction
    {
        public const string Name = "AddToInventory";

        [JsonConstructor]
        private AddToInventoryAction()
        {
        }

        public AddToInventoryAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            InventoryItemId = args[0];
            Description = args[1];
        }

        [JsonProperty]
        public string InventoryItemId { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        public override CommandActionResult Execute(DialogContext dc, IList<IActivity> activities, GameFlags flags)
        {
            flags.SetFlag(InventoryItemId);

            activities.Add(CreateEventActivity(dc, "InventoryItemAdded", JObject.FromObject(new
            {
                inventoryItemId = InventoryItemId,
                description = Description
            })));

            return CommandActionResult.None;
        }
    }
}