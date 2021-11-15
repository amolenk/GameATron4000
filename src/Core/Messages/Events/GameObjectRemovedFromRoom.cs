namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectRemovedFromRoom(IGameObject GameObject, Room Room)
    :IEvent;
