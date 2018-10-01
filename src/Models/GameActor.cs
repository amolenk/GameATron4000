using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GameActor : GameObject
    {
        [JsonProperty("textColor")]

        public string TextColor { get; set; }
    }
}