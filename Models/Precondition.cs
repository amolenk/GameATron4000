namespace GameATron4000.Models
{
    public class Precondition
    {
        public Precondition(string flag, bool value)
        {
            this.Flag = flag;
            this.Value = value;
        }

        public string Flag { get; }

        public bool Value { get; }
    }
}