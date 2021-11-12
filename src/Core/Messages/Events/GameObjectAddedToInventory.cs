namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectAddedToInventory(
    GameObject GameObject,
    Actor NewOwner)
    : IEvent;
