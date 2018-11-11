using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Games;
using GameATron4000.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameATron4000.Pages
{
    public class PlayModel : PageModel
    {
        private const string DirectLineServiceName = "directline";

        public string GameTitle { get; private set; }

        public string OptionsJson { get; private set; }

        public string GameInfoJson { get; private set; }

        public PlayModel(BotConfiguration botConfig, IOptions<BotOptions> botOptionsAccessor, IOptions<GuiOptions> guiOptionsAccessor)
        {
            var service = botConfig.Services.FirstOrDefault(s => s.Name == DirectLineServiceName);
            if (!(service is GenericService genericService))
            {
                throw new InvalidOperationException($"The .bot file does not contain a generic service with name '{DirectLineServiceName}'.");
            }

            var guiOptions = guiOptionsAccessor.Value;

            OptionsJson = JsonConvert.SerializeObject(new
            {
                botId = genericService.Configuration["BotId"],
                directLineSecret = genericService.Configuration["DirectLineSecret"],
                enableFullScreen = guiOptions.EnableFullScreen,
                minTextDuration = guiOptions.MinTextDuration,
                textSpeed = guiOptions.TextSpeed
            });
        }

        public void OnGet(string game)
        {
            var gameCatalog = new GameCatalog("Games");
            var gameInfo = gameCatalog.GetGameInfo(game);

            GameTitle = gameInfo.Title;
            GameInfoJson = JsonConvert.SerializeObject(new
            {
                gameName = game,
                playerActor = gameInfo.PlayerActor,
                assets = gameInfo.Assets
                    .Select(asset => new
                    {
                        key = asset.Key,
                        url = $"/dist/games/{game}{asset.Url}",
                        frameWidth = asset.FrameWidth,
                        frameHeight = asset.FrameHeight
                    })
            });
        }
    }
}
