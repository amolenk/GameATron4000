namespace Amolenk.GameATron4000.Messages.Events;

public record ObjectRemovedFromRoom(GameObject GameObject, Room Room) : IEvent;
