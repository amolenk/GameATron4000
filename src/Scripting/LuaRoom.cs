using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaRoom : IRoom
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaRoom(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaRoom FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaRoom(table, script);
        }

        public string Id
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Id); }
        }

        public IEnumerable<Point> Walkbox
        {
            get
            {
                var points = _luaTable.GetTable(LuaConstants.Tables.Room.Walkbox);
                if (points != null)
                {
                    foreach (LuaTable point in points.Values)
                    {
                        // Lua indices are 1-based.
                        yield return new Point(point.GetNumber(1).Value, point.GetNumber(2).Value);
                    }
                }
            }
        }

        public IEnumerable<IObject> GetObjects()
        {
            return _script.Objects.Where(o => o.RoomId == Id);
        }
    }
}