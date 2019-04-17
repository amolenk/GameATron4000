using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaObjectDependency : IObjectDependency
    {
        private readonly LuaTable _luaTable;
        private readonly LuaGameScript _script;

        private LuaObjectDependency(LuaTable luaTable, LuaGameScript script)
        {
            _luaTable = luaTable;
            _script = script;
        }

        public static LuaObjectDependency FromTable(LuaTable table, LuaGameScript script)
        {
            return new LuaObjectDependency(table, script);
        }

        public IObject Object
        {
            get
            {
                var objectTable = _luaTable.GetTable(LuaConstants.Tables.ObjectDependency.Object);
                if (objectTable != null)
                {
                    return LuaObject.FromTable(objectTable, _script);
                }
                return null; // TODO Throw?
            }
        }

        public string State
        {
            get { return _luaTable.GetString(LuaConstants.Tables.ObjectDependency.State); }
        }
    }
}