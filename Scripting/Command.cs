using System.Collections.Generic;

namespace GameATron4000.Scripting
{
    public class Command
    {
        public const string EnterRoom = "<EnterRoom>";
        
        public Command(string text, List<CommandAction> actions)
        {
            this.Text = text;
            this.Actions = actions;
        }

        public string Text { get; }

        public IEnumerable<CommandAction> Actions { get; }
    }
}