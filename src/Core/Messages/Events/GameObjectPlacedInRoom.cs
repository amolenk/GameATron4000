namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectPlacedInRoom(GameObjectSnapshot GameObject, Room Room)
    : IEvent;
