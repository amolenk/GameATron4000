using System.Collections.Generic;
using System.Linq;
using GameATron4000.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace GameATron4000.Models.Actions
{
    public class AddToInventoryAction : RoomAction
    {
        public const string Name = "AddToInventory";

        private readonly string _inventoryItemId;
        private readonly string _description;

        public AddToInventoryAction(string inventoryItemId, string description, IEnumerable<Precondition> preconditions = null)
            : base(preconditions != null ? preconditions.ToArray() : new Precondition[0])
        {
            _inventoryItemId = inventoryItemId;
            _description = description;
        }

        public AddToInventoryAction(List<string> args, Precondition[] preconditions)
            : base(preconditions)
        {
            _inventoryItemId = args[0];
            _description = args[1];
        }

        public override string Execute(DialogContext dc, IList<IActivity> activities, IDictionary<string, object> state) {

            state["flag_" + _inventoryItemId] = true;

            activities.Add(CreateEventActivity(dc, "InventoryItemAdded", JObject.FromObject(new
            {
                inventoryItemId = _inventoryItemId,
                description = _description
            })));

            return string.Empty;
        }
    }
}