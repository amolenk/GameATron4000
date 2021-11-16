namespace Amolenk.GameATron4000.Messages.Events;

public record ProtagonistChanged : IEvent
{
    public Actor Protagonist { get; }

    public IEnumerable<Item> InventoryItems { get; }

    public ProtagonistChanged(Actor protagonist)
    {
        Protagonist = protagonist;
        InventoryItems = new List<Item>(protagonist.GetInventoryItems());
    }
}
