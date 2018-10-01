using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GameInventoryItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}