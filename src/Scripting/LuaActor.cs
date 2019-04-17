using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaActor : IActor
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaActor(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaActor FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaActor(table, script);
        }

        public string Id
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Id); }
        }

        public string Name
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Name); }
            set { _luaTable[LuaConstants.Tables.Name] = value; }
        }

        public string[] Classes
        {
            get
            {
                var classesTable = _luaTable.GetTable(LuaConstants.Tables.Actor.Classes);
                if (classesTable != null)
                {
                    return classesTable.ToEnumerable()
                        .Select(entry => entry.Value?.ToString())
                        .ToArray();
                }

                return new string[0];
            }
        }

        public string TextColor
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Actor.TextColor); }
        }

        public string RoomId
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.RoomId); }
            set { _luaTable[LuaConstants.Tables.Object.RoomId] = value; }
        }

        public int? PositionX
        {
            get { return (int?)_luaTable.GetNumber(LuaConstants.Tables.Object.PositionX); }
            set { _luaTable[LuaConstants.Tables.Object.PositionX] = (double?)value; }
        }

        public int? PositionY
        {
            get { return (int?)_luaTable.GetNumber(LuaConstants.Tables.Object.PositionY); }
            set { _luaTable[LuaConstants.Tables.Object.PositionY] = (double?)value; }
        }

        public IEnumerable<IObject> GetInventory()
        {
            return _script.Objects.Where(o => o.Owner == Id);
        }
    }
}