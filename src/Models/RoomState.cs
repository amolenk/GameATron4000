using System.Collections.Generic;
using GameATron4000.Scripting;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Models
{
    public class RoomState
    {
        public Dictionary<string, ActorState> Actors { get; set; } = new Dictionary<string, ActorState>();

        public Dictionary<string, ObjectState> Objects { get; set; } = new Dictionary<string, ObjectState>();
    }

    public class ActorState
    {
        public int X { get; set; }

        public int Y { get; set; }
    }

    public class ObjectState
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string State { get; set; }
    }
}