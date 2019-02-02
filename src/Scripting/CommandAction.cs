using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameATron4000.Scripting
{
    public abstract class CommandAction
    {
        protected CommandAction()
        {
        }

        protected CommandAction(List<ActionPrecondition> preconditions)
        {
            Preconditions = preconditions ?? new List<ActionPrecondition>();
        }
        
        [JsonProperty]
        public List<ActionPrecondition> Preconditions { get; private set; }
    }
}