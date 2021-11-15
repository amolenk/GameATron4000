namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectPlacedInRoom(IGameObject GameObject, Room Room) : IEvent;
