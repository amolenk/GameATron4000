using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Games;
using GameATron4000.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace GameATron4000.Pages
{
    public class PlayModel : PageModel
    {
        public string GameAssetsJson { get; private set; }
    
        public void OnGet(string game)
        {
            var gameCatalog = new GameCatalog("Games");
            var gameInfo = gameCatalog.LoadGame(game);

            GameAssetsJson = JsonConvert.SerializeObject(gameInfo.Assets);
        }
    }
}
