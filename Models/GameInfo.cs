using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GameInfo
    {
        public string Title { get; set; }
        
        public string InitialRoom { get; set; }

        public bool SupportsGui { get; set; }

        public string PlayerActor { get; set; }

        public string[] BadCommandResponses { get; set; }

        public Dictionary<string, GameActor> Actors { get; set; }

        public Dictionary<string, GameObject> Objects { get; set; }

        public List<string> InitialInventory { get; set; }

        public Dictionary<string, RoomState> InitialRoomStates { get; set; }

        public List<GameAsset> Assets { get; set; }

        public List<GameScript> RoomScripts { get; set; }

        public List<GameScript> ConversationScripts { get; set; }
    }

    public class GameObject
    {
        public string Description { get; set; }
    }

    public class GameActor : GameObject
    {
        public string TextColor { get; set; }
    }

    public class RoomState
    {
        public Dictionary<string, Placement> ActorPlacements { get; set; } = new Dictionary<string, Placement>();

        public Dictionary<string, Placement> ObjectPlacements { get; set; } = new Dictionary<string, Placement>();
    }

}