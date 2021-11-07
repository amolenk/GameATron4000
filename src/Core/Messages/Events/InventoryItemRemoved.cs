namespace Amolenk.GameATron4000.Messages.Events;

public record InventoryItemRemoved(Actor Actor, GameObject GameObject) : IEvent;
