using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaScale : IScale
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaScale(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaScale FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaScale(table, script);
        }

        public int? Min
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Scale.Min); }
        }

        public int? Max
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Scale.Max); }
        }

        public int? StartPos
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Scale.Start); }
        }

        public int? EndPos
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Scale.End); }
        }
    }
}