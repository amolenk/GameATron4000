using System.Collections.Generic;

namespace GameATron4000.Models
{
    public class Command
    {
        public const string RoomEntered = "<RoomEntered>";

        private static readonly IEnumerable<Precondition> EmptyPreconditions = new Precondition[0];
        
        public Command(string text, List<RoomAction> actions, List<Precondition> preconditions = null)
        {
            this.Text = text;
            this.Actions = actions;
            this.Preconditions = preconditions ?? EmptyPreconditions;
        }

        public string Text { get; }

        public IEnumerable<RoomAction> Actions { get; }

        public IEnumerable<Precondition> Preconditions { get; }
    }
}