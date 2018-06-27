using System.Collections.Generic;
using System.Linq;

namespace GameATron4000.Models
{
    public class Action
    {
        public const string AddToInventory = "AddToInventory";
        public const string Speak = "Speak";
        public const string Describe = "Describe";

        public Action(string name, string[] args, List<Precondition> preconditions = null)
        {
            this.Name = name;
            this.Args = args ?? new string[0];
            this.Preconditions = preconditions ?? Enumerable.Empty<Precondition>();
        }

        public string Name { get; } 

        public string[] Args { get; }

        public IEnumerable<Precondition> Preconditions { get; }
    }
}