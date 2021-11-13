namespace Amolenk.GameATron4000.Messages.Events;

public record ProtagonistChanged(
    Actor Protagonist,
    IEnumerable<Item> InventoryItems)
    : IEvent;
