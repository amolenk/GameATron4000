namespace Amolenk.GameATron4000.Model.Builders;

public class RoomBuilder
{
    public string Id { get; private set; }
    public Polygon? WalkboxArea { get; private set; }
    
    public RoomHandlersBuilder When { get; private set; }
    internal Game Game { get; private set; }

    internal RoomBuilder(string id, Game game)
    {
        Id = id;
        When = new(this);
        Game = game;
    }

    public RoomBuilder WithWalkboxArea(params Point[] vertices)
    {
        WalkboxArea = new Polygon(vertices);
        return this;
    }

    internal Room Build()
    {
        if (WalkboxArea is null)
        {
            throw new InvalidOperationException(
                "Walkbox area must be set for a room.");
        }

        return new Room(
            Id,
            Game,
            new Walkbox(WalkboxArea),
            new RoomHandlers(
                When.HandleBeforeEnter,
                When.HandleAfterEnter));
    }
}