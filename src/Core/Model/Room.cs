namespace Amolenk.GameATron4000.Model;

public class Room
{
    private readonly Game _game;

    public string Id { get; }
    public Walkbox Walkbox { get; }
    internal RoomHandlers Handlers { get; private set; }
    protected RoomState State { get; }

    internal Room(RoomBuilder builder)
    {
        if (builder.WalkboxArea is null)
        {
            throw new ArgumentException(
                "Walkbox area must be set for a room.",
                nameof(builder));
        }

        _game = builder.Game;
    
        Id = builder.Id;
        Walkbox = new(builder.WalkboxArea);
        State = new();
        Handlers = new(builder.When);
    }

    public void Enter()
    {
        if (Handlers.HandleBeforeEnter is not null)
        {
            // Do not enqueue events while calling the BeforeEnter handler.
            // Any changes happening in the handler should not be made visible
            // in the UI yet.
            var originalIgnoreEvents = _game.EventQueue.IgnoreNewEvents;
            _game.EventQueue.IgnoreNewEvents = true;

            Handlers.HandleBeforeEnter();

            _game.EventQueue.IgnoreNewEvents = originalIgnoreEvents;
        }

        _game.EventQueue.Enqueue(new RoomEntered(
            this,
            GetVisibleObjects().ToList()));

        _game.NotifyRoomEntered(this);
    }

    public void Place(GameObject gameObject, double x, double y)
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
            gameObject.ClearOwner();
        }

        // Update state.
        State.Objects.Add(gameObject);
        gameObject.NotifyPlacedInRoom(this, new Point(x, y));

        _game.EventQueue.Enqueue(new GameObjectPlacedInRoom(
            gameObject,
            this));
    }

    public void Remove(GameObject gameObject)
    {
        if (State.Objects.Contains(gameObject))
        {
            State.Objects.Remove(gameObject);

            _game.EventQueue.Enqueue(new GameObjectRemovedFromRoom(
                gameObject,
                this));
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

    internal IEnumerable<GameObject> GetVisibleObjects() =>
        State.Objects.Where(gameObject => gameObject.IsVisible);
}
