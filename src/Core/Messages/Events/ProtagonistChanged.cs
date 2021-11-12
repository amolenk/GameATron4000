namespace Amolenk.GameATron4000.Messages.Events;

public record ProtagonistChanged(
    Actor Protagonist,
    List<GameObject> InventoryItems)
    : IEvent;
