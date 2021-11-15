namespace Amolenk.GameATron4000.Model;

public class Room
{
    private readonly Game _game;
    private readonly List<IGameObject> _objects;

    public string Id { get; }
    public Walkbox Walkbox { get; }

    internal RoomHandlers Handlers { get; private set; }

    internal Room(
        string id,
        Game game,
        Walkbox walkbox,
        RoomHandlers handlers)
    {
        _game = game;
        _objects = new();

        Id = id;
        Walkbox = walkbox;
        Handlers = handlers;
    }

    public void Place(IGameObject gameObject, double x, double y) =>
        Place(gameObject, new Point(x, y));

    public void Place(IGameObject gameObject, Point position)
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
        gameObject.UpdatePosition(position);
        _objects.Add(gameObject);

        _game.EventQueue.Enqueue(new GameObjectPlacedInRoom(
            gameObject,
            this));
    }

    public void Remove(IGameObject gameObject)
    {
        if (_objects.Contains(gameObject))
        {
            _objects.Remove(gameObject);

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

    internal bool ContainsObject(IGameObject gameObject) =>
        _objects.Contains(gameObject);

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
        
        Handlers.HandleAfterEnter?.Invoke();
    }

    internal IEnumerable<IGameObject> GetVisibleObjects() =>
        _objects.Where(gameObject => gameObject.IsVisible);

    internal RoomSnapshot Save() => new RoomSnapshot(
        _objects.Select(gameObject => gameObject.Id).ToList());

    internal void Restore(RoomSnapshot snapshot)
    {
        if (snapshot.Objects is not null)
        {
            _objects.Clear();

            foreach (var id in snapshot.Objects)
            {
                if (_game.TryGetItem(id, out Item item))
                {
                    _objects.Add(item);
                }
                else if (_game.TryGetActor(id, out Actor actor))
                {
                    _objects.Add(actor);
                }
            }
        }
    }
}
