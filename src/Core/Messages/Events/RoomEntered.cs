namespace Amolenk.GameATron4000.Messages.Events;

public record RoomEntered(Room Room, List<GameObject> VisibleObjects) : IEvent;
