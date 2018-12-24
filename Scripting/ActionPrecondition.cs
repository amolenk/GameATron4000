namespace GameATron4000.Scripting
{
    public class ActionPrecondition
    {
        public ActionPrecondition(string flagOrInventoryItemId, bool inverted)
        {
            this.FlagOrInventoryItemId = flagOrInventoryItemId;
            this.Inverted = inverted;
        }

        public string FlagOrInventoryItemId { get; }

        public bool Inverted { get; }
    }
}