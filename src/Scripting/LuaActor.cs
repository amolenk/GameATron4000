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
            get { return _luaTable.GetNumber(LuaConstants.Tables.Object.PositionX); }
            set { _luaTable[LuaConstants.Tables.Object.PositionX] = value; }
        }

        public int? PositionY
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Object.PositionY); }
            set { _luaTable[LuaConstants.Tables.Object.PositionY] = value; }
        }

        public string UsePosition
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.UsePosition); } // TODO
        }

        public string UseDirection
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.UseDirection); } // TODO
        }

        public string FaceDirection
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.FaceDirection); } // TODO
            set { _luaTable[LuaConstants.Tables.Object.FaceDirection] = value; }
        }

        public IEnumerable<IObject> GetInventory()
        {
            return _script.Objects.Where(o => o.Owner == Id);
        }
    }
}