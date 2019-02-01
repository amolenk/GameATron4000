using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameATron4000.Pages
{
    public class IndexModel : PageModel
    {
        public string OptionsJson { get; private set; }

        public string GameInfoJson { get; private set; }

        public IndexModel(BotServices botServices, IOptions<GuiOptions> guiOptionsAccessor)
        {
            var guiOptions = guiOptionsAccessor.Value;

            OptionsJson = JsonConvert.SerializeObject(new
            {
                botId = botServices.BotId,
                directLineSecret = botServices.DirectLineSecret,
                enableFullScreen = guiOptions.EnableFullScreen,
                minTextDuration = guiOptions.MinTextDuration,
                textSpeed = guiOptions.TextSpeed
            });
        }

        public void OnGet()
        {
            // Load the metadata for the game.
            var gameInfoJson = System.IO.File.ReadAllText("Gameplay/game.json");
            var gameInfo = JsonConvert.DeserializeObject<GameInfo>(gameInfoJson);

            GameInfoJson = JsonConvert.SerializeObject(new
            {
                playerActor = gameInfo.PlayerActor,
                assets = gameInfo.Assets
                    .Select(asset => new
                    {
                        key = asset.Key,
                        url = $"/dist/gameplay{asset.Value.Url}",
                        frameWidth = asset.Value.FrameWidth,
                        frameHeight = asset.Value.FrameHeight
                    })
            });
        }
    }
}
