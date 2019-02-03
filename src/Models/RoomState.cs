using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class RoomState
    {
        public Dictionary<string, ActorPosition> ActorPositions { get; set; } = new Dictionary<string, ActorPosition>();

        public Dictionary<string, ObjectPosition> ObjectPositions { get; set; } = new Dictionary<string, ObjectPosition>();
    }
}