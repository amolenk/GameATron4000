namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectRemovedFromRoom(GameObject GameObject, Room Room)
    :IEvent;
