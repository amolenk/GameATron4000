using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class ConversationStep
    {
        public ConversationStep(List<Action> actions, Dictionary<string, ConversationStep> subSteps)
        {
            this.Actions = actions;
            this.SubSteps = subSteps;
        }

        public string Text { get; }

        public IEnumerable<Action> Actions { get; }

        public Dictionary<string, ConversationStep> SubSteps { get; }
    }
}