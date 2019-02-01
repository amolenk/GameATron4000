using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameATron4000.Models
{
    public class ActorPosition: ObjectPosition
    {
        public ActorPosition(int x, int y,
            ActorDirection direction = ActorDirection.Front, bool foreground = false)
            : base(x, y, foreground)
        {
            Direction = direction;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ActorDirection Direction { get; set; }
    }
}