using System.Collections.Generic;
using GameATron4000.Models;

namespace GameATron4000.Extensions
{
    public static class StateExtensions
    {
        public static void SetFlag(this IDictionary<string, object> state, string flagName)
        {
            var key = GetKey(flagName);

            state[key] = true;
        }

        public static void ClearFlag(this IDictionary<string, object> state, string flagName)
        {
            var key = GetKey(flagName);

            if (state.ContainsKey(key))
            {
                state.Remove(key);
            }
        }

        public static bool SatifiesPrecondition(this IDictionary<string, object> state, Precondition precondition)
        {
            var key = GetKey(precondition.Flag);

            return precondition.Value ? state.ContainsKey(key) : !state.ContainsKey(key);
        }

        private static string GetKey(string flagName)
        {
            return "flag_" + flagName;
        }
    }
}