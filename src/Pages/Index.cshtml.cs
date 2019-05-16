using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Configuration;
using GameATron4000.Core;
using GameATron4000.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameATron4000.Pages
{
    public class IndexModel : PageModel
    {
        public string GameInfoJson { get; private set; }

        public IndexModel(BotServices botServices, IOptions<GuiOptions> guiOptionsAccessor)
        {
            var guiOptions = guiOptionsAccessor.Value;

            GameInfoJson = JsonConvert.SerializeObject(new
            {
                botId = botServices.BotId,
                directLineSecret = botServices.DirectLineSecret,
                minTextDuration = guiOptions.MinTextDuration,
                textSpeed = guiOptions.TextSpeed,
                assets = new string[] { "sprites" }
            });
        }
    }
}
