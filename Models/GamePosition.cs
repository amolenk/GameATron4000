using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;

namespace GameATron4000.Models
{
    public class GamePosition
    {
        public GamePosition(int x, int y, bool foreground = false)
        {
            X = x;
            Y = y;
            Foreground = foreground;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool Foreground { get; set; }
    }
}