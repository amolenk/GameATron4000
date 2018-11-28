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

        public List<GameAsset> Assets { get; set; }

        public List<GameScript> RoomScripts { get; set; }

        public List<GameScript> ConversationScripts { get; set; }
    }
}