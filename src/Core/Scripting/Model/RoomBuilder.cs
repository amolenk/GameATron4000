namespace Amolenk.GameATron4000.Scripting.Model;

public class RoomBuilder
{
    private readonly ICollector<Room> _rooms;

    public string Id { get; private set; }
    public Polygon? WalkboxArea { get; private set; } 

    internal RoomBuilder(
        string id,
        ICollector<Room> rooms)
    {
        _rooms = rooms;

        Id = id;
    }

    public RoomBuilder WithWalkboxArea(params Point[] vertices)
    {
        WalkboxArea = new Polygon(vertices);
        return this;
    }

    public Room Add()
    {
        var room = new Room(this);

        _rooms.Add(room);

        return room;
    }
}