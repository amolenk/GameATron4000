using System;
using System.Collections.Generic;
using System.Linq;
using GameATron4000.Core;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaGameScriptState : Dictionary<string, Dictionary<string, object>>, IGameScriptState
    {
        private const string TABLE_KEY_TYPE = "type";

        public LuaGameScriptState()
        {
        }

        public static LuaGameScriptState Save(Lua lua)
        {
            var result = new LuaGameScriptState();
            var ignoreKeys = new string[] { "selected_actor", "current_room", "prev_room" };

            var tablesToSave = ((LuaTable)lua["_G"])
                .ToEnumerable()
                .Where(g => !ignoreKeys.Contains(g.Key) && g.Value is LuaTable)
                .Select(g => new
                {
                    Key = g.Key,
                    Table = (LuaTable)g.Value
                })
                .Where(x => x.Table[TABLE_KEY_TYPE] is string);

            foreach (var entry in tablesToSave)
            {
                var tableState = entry.Table.ToEnumerable()
                    .Where(row => row.Value is string || row.Value is double)
                    .ToDictionary(row => row.Key, row => row.Value);

                result.Add(entry.Key, tableState);
            }

            return result;
        }

        public void Load(Lua lua)
        {
            foreach (var entry in this)
            {
                var table = lua[entry.Key] as LuaTable;
                if (table != null)
                {
                    foreach (var row in entry.Value)
                    {
                        table[row.Key] = row.Value;
                    }
                }
            }
        }

        public LuaGameScriptState Except(LuaGameScriptState other)
        {
            var result = new LuaGameScriptState();

            foreach (var entry in this)
            {
                if (other.ContainsKey(entry.Key))
                {
                    var diff = entry.Value.Except(other[entry.Key]);
                    if (diff.Any())
                    {
                        result.Add(entry.Key, diff.ToDictionary(d => d.Key, d => d.Value));
                    }
                }
                else
                {
                    result.Add(entry.Key, entry.Value);
                }
            }

            return result;
        }
    }
}