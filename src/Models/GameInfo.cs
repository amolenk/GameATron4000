using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GameInfo
    {
        public string InitialRoom { get; set; }

        public bool SupportsGui { get; set; }

        public string PlayerAvatar { get; set; }

        public string[] BadCommandResponses { get; set; }

        [JsonIgnore]
        public DialogSet Dialogs { get; set; }

        public List<GameAsset> Assets { get; set; }
    }
}