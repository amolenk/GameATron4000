using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class GameFlags : Dictionary<string, bool>
    {
        public void SetFlag(string flagName)
        {
            var key = GetKey(flagName);

            this[key] = true;
        }

        public void ClearFlag(string flagName)
        {
            var key = GetKey(flagName);

            if (ContainsKey(key))
            {
                Remove(key);
            }
        }

        public bool SatisfyPrecondition(Precondition precondition)
        {
            var key = GetKey(precondition.Flag);

            return precondition.Value ? ContainsKey(key) : !ContainsKey(key);
        }

        private static string GetKey(string flagName)
        {
            return "flag_" + flagName;
        }
    }
}