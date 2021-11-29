namespace Amolenk.GameATron4000.Messages.Events;

public record RoomEntered : IEvent
{
    public Room Room { get; }

    public List<GameObjectSnapshot> VisibleObjects { get; }

    public RoomEntered(Room room)
    {
        Room = room;
        VisibleObjects = room.GetVisibleObjects()
            .Select(gameObject => new GameObjectSnapshot(gameObject))
            .ToList();
    }
}
