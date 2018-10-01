using Newtonsoft.Json.Linq;

namespace GameATron4000.Models
{
    public class GameRoom
    {
        public string Id { get; set; }

        public GameActor[] Actors { get; set; }

        public GameInventoryItem[] InventoryItems { get; set; }

        public GameObject[] Objects { get; set; }

        public JObject ToJObject()
        {
            return JObject.FromObject(new
            {
                roomId = Id,
                actors = Actors,
                inventoryItems = InventoryItems,
                objects = Objects
            });
        }
    }
}