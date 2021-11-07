namespace Amolenk.GameATron4000.Messages.Events;

public record ObjectPlacedInRoom(GameObject GameObject, Room Room) : IEvent;
