namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectPlacedInRoom(GameObject GameObject, Room Room) : IEvent;
