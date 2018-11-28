﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameATron4000.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameATron4000.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<string> GameNames { get; private set; }

        public IndexModel(GameCatalog gameCatalog)
        {
            GameNames = gameCatalog.GetGameNames().OrderBy(n => n);
        }
    }
}
