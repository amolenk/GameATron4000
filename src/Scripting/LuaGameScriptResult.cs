using System.Collections.Generic;
using GameATron4000.Core;
using Microsoft.Bot.Schema;

namespace GameATron4000.Scripting
{
    public class LuaGameScriptResult : IGameScriptResult
    {
        public static readonly LuaGameScriptResult Empty = new LuaGameScriptResult();

        public List<Activity> Activities { get; } = new List<Activity>();

        public string NextDialogId { get; set; } = string.Empty;
    }
}