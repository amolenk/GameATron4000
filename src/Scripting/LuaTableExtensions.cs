using System.Collections.Generic;
using System.Linq;
using NLua;

namespace GameATron4000.Scripting
{
    public static class LuaTableExtensions
    {
        public static bool? GetBoolean(this LuaTable table, string key)
        {
            return table[key] as bool?;
        }

        public static string GetString(this LuaTable table, string key)
        {
            return table[key]?.ToString() ?? string.Empty;
        }

        public static double? GetNumber(this LuaTable table, string key)
        {
            return table[key] as double?;
        }

        public static LuaTable GetTable(this LuaTable table, string key)
        {
            return table[key] as LuaTable;
        }

        public static IEnumerable<KeyValuePair<string, object>> ToEnumerable(this LuaTable table)
        {
            foreach (KeyValuePair<object, object> entry in table)
            {
                yield return new KeyValuePair<string, object>(entry.Key.ToString(), entry.Value);
            }
        }

        public static Dictionary<string, object> SaveState(this LuaTable table, params string[] excludeKeys)
        {
            var result = new Dictionary<string, object>();

            return table.ToEnumerable()
                .Where(row => !excludeKeys.Contains(row.Key))
                .ToDictionary(row => row.Key, row => row.Value);
        }
    }
}