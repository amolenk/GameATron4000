namespace Amolenk.GameATron4000.Model;

public class Room
{
    private readonly Game _game;
    private readonly List<GameObject> _objects;

    public string Id { get; }
    public Polygon WalkboxArea { get; }
    public IReadOnlyList<GameObject> Objects => _objects.AsReadOnly();
    internal RoomHandlers Handlers { get; private set; }

    internal Room(RoomBuilder builder)
    {
        if (builder.WalkboxArea is null)
        {
            throw new ArgumentException(
                "Walkbox area must be set for a room.",
                nameof(builder));
        }

        _game = builder.Game;
        _objects = new List<GameObject>();

        Id = builder.Id;
        WalkboxArea = builder.WalkboxArea;
        Handlers = new(builder.When);
    }

    public void Enter()
    {
        if (Handlers.HandleBeforeEnter is not null)
        {
            // Do not enqueue events while calling the BeforeEnter handler.
            // Any changes happening in the handler should not be made visible
            // in the UI yet.
            _game.EventQueue.IgnoreNewEvents = true;

            Handlers.HandleBeforeEnter();

            _game.EventQueue.IgnoreNewEvents = false;
        }

        _game.EventQueue.Enqueue(new EnterRoomActionExecuted(this));

        _game.NotifyRoomEntered(this);
    }

    public void Place(GameObject gameObject, int x, int y)
    {
        // If the object is currently in another room, remove it.
        if (gameObject.Room is not null)
        {
            gameObject.Room.Remove(gameObject);
        }
        // Otherwise, if the object is currently owned by an actor, clear the
        // owner.
        else if (gameObject.Owner is not null)
        {
            _game.EventQueue.Enqueue(new InventoryItemRemoved(
                gameObject.Owner, gameObject));

            gameObject.Owner = null;
        }

        // Add the object to this room.
        gameObject.Position = new Point(x, y);
        gameObject.Room = this;
        _objects.Add(gameObject);

        _game.EventQueue.Enqueue(new ObjectPlacedInRoom(gameObject, this));
    }

    public void Remove(GameObject gameObject)
    {
        if (_objects.Remove(gameObject))
        {
            gameObject.Room = null;

            _game.EventQueue.Enqueue(new ObjectRemovedFromRoom(
                gameObject, this));
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is Room room)
        {
            return Id.Equals(room.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
