using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaObject : IObject
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaObject(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaObject FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaObject(table, script);
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
                var classesTable = _luaTable.GetTable(LuaConstants.Tables.Object.Classes);
                if (classesTable != null)
                {
                    return classesTable.ToEnumerable()
                        .Select(entry => entry.Value?.ToString())
                        .ToArray();
                }

                return new string[0];
            }
        }

        public string Owner
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.Owner); }
            set { _luaTable[LuaConstants.Tables.Object.Owner] = value; }
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

        public int? ZOffset
        {
            get { return _luaTable.GetNumber(LuaConstants.Tables.Object.ZOffset); }
            set { _luaTable[LuaConstants.Tables.Object.ZOffset] = value; }
        }

        public string State
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.State); }
            set { _luaTable[LuaConstants.Tables.Object.State] = value; }
        }

        public string UsePosition
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.UsePosition); }
        }

        public string UseDirection
        {
            get { return _luaTable.GetString(LuaConstants.Tables.Object.UseDirection); } 
        }

        public IObjectDependency DependsOn
        {
            get
            {
                var dependsOnTable = _luaTable.GetString(LuaConstants.Tables.Object.DependsOn);
                if (dependsOnTable != null)
                {
                    return LuaObjectDependency.FromString(dependsOnTable, _script);
                }
                return null;
            }
        }

        public bool IsVisible
        {
            get
            {
                var dependsOn = this.DependsOn;
                if (dependsOn == null)
                {
                    return true;
                }

                return dependsOn.Object.State == dependsOn.State;
            }
        }
    }
}