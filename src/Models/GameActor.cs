using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Models
{
    public class GameActor
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }        
        public string Name { get; set; }
        public string TextColor { get; set; }

        public static GameActor FromLuaTable(LuaTable table)
        {
            return new GameActor
            {
                Id = table["id"].ToString(),
                Name = table["name"]?.ToString(),
                RoomId = table["room_id"]?.ToString(),
                X = table["x"] != null ? (int)(double)table["x"] : (int?)null,
                Y = table["y"] != null ? (int)(double)table["y"] : (int?)null,
                TextColor = table["text_col"]?.ToString() ?? "White"
            };
        }
    }
}