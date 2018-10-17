using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class Command
    {
        public const string InitializeRoom = "<InitializeRoom>";

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