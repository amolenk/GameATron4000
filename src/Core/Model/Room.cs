namespace Amolenk.GameATron4000.Model;

public class Room
{
    private readonly Game _game;
    private RoomState _state { get; }

    public string Id { get; }
    public Walkbox Walkbox { get; }
    public IEnumerable<GameObject> Objects => _state.Objects;
    internal RoomHandlers Handlers { get; private set; }

    internal Room(string id, Game game, Walkbox walkbox, RoomHandlers handlers)
    {
        _game = game;
        _state = new();

        Id = id;
        Walkbox = walkbox;
        Handlers = handlers;
    }

    

    public void Place(GameObject gameObject, double x, double y)
    {
        // If the object is currently in another room, remove it.
        if (_game.TryGetRoomForObject(gameObject, out Room room))
        {
            room.Remove(gameObject);
        }
        // Otherwise, if the object is currently owned by an actor, clear the
        // owner.
        else if (gameObject is Item item &&
            _game.TryGetOwnerForItem(item, out Actor actor))
        {
            actor.RemoveFromInventory(item);
        }

        // Update state.
        gameObject.SetPosition(new Point(x, y));
        _state.Objects.Add(gameObject);

        _game.EventQueue.Enqueue(new GameObjectPlacedInRoom(
            gameObject,
            this));
    }

    public void Remove(GameObject gameObject)
    {
        if (Objects.Contains(gameObject))
        {
            _state.Objects.Remove(gameObject);

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

    internal void Enter()
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
    }

    internal IEnumerable<GameObject> GetVisibleObjects() =>
        Objects.Where(gameObject => gameObject.IsVisible);
}
