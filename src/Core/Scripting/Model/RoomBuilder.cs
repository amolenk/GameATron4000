namespace Amolenk.GameATron4000.Scripting.Model;

public class RoomBuilder
{
    public string Id { get; private set; }
    public Polygon? WalkboxArea { get; private set; }
    public RoomActionHandlers ActionHandlers { get; private set; }
    internal Game Game { get; private set; }

    internal RoomBuilder(string id, Game game)
    {
        Id = id;
        ActionHandlers = new();
        Game = game;
    }

    public RoomBuilder WithWalkboxArea(params Point[] vertices)
    {
        WalkboxArea = new Polygon(vertices);
        return this;
    }

    public RoomBuilder When(Action<RoomActionHandlers> configure)
    {
        configure(ActionHandlers);
        return this;
    }

    public Room Add()
    {
        var room = new Room(this);

//        Game.NotifyRoomAdded(room);

        return room;
    }
}