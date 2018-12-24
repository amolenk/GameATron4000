using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class RoomState
    {
        public Dictionary<string, GamePosition> ActorPositions { get; set; } = new Dictionary<string, GamePosition>();

        public Dictionary<string, GamePosition> ObjectPositions { get; set; } = new Dictionary<string, GamePosition>();
    }
}