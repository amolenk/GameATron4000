namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectRemovedFromInventory(
    GameObject GameObject,
    Actor PreviousOwner)
    : IEvent;
