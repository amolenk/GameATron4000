using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GameInfo
    {
        public string InitialRoom { get; set; }

        // TODO Add Id suffix...
        public string PlayerActor { get; set; }

        // TODO Rename to Canned...
        public string[] BadCommandResponses { get; set; }

        public Dictionary<string, GameActor> Actors { get; set; }

        public Dictionary<string, GameObject> Objects { get; set; }

        public List<string> InitialInventory { get; set; }

        public Dictionary<string, RoomState> InitialRoomStates { get; set; }

        public Dictionary<string, string> RoomScripts { get; set; }

        public Dictionary<string, string> ConversationScripts { get; set; }

        public Dictionary<string, GameAsset> Assets { get; set; }
    }
}