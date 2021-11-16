namespace Amolenk.GameATron4000.Model;

public class Room
{
    private readonly Game _game;
    private readonly List<GameObject> _objects;

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

    public void Place(GameObject gameObject, double x, double y) =>
        Place(gameObject, new Point(x, y));

    public void Place(GameObject gameObject, Point position)
    {
        // If the object is currently in another room, remove it.
        if (_game.TryGetRoomForObject(gameObject, out Room room))
        {
            room.Remove(gameObject);
        }
        // Otherwise, if the object is currently owned by an actor, clear the
        // owner.
        else if (gameObject is Item item &&
            _game.TryGetOwnerForItem(item, out Actor owner))
        {
            owner.RemoveFromInventory(item);
        }

        // Update state.
        gameObject.UpdatePosition(position);
        _objects.Add(gameObject);

        _game.EventQueue.Enqueue(new GameObjectPlacedInRoom(
            new GameObjectSnapshot(gameObject),
            this));
    }

    public void Remove(GameObject gameObject)
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

    internal bool ContainsObject(GameObject gameObject) =>
        _objects.Contains(gameObject);

    internal void Enter()
    {
        if (Handlers.HandleBeforeEnter is not null)
        {
            // Do not enqueue events while calling the BeforeEnter handler.
            // Any changes happening in the handler should not be made visible
            // in the UI yet.
            var originalFilter = _game.EventQueue.Filter;
            _game.EventQueue.IgnoreAll();

            Handlers.HandleBeforeEnter();

            _game.EventQueue.SetFilter(originalFilter);
        }

        _game.EventQueue.Enqueue(new RoomEntered(this));
        
        Handlers.HandleAfterEnter?.Invoke();
    }

    internal IEnumerable<GameObject> GetVisibleObjects() =>
        _objects.Where(gameObject => gameObject is Actor
            || gameObject is Item { IsVisible: true });

    internal RoomState Save() => new RoomState(
        _objects.Select(gameObject => gameObject.Id).ToList());

    internal void Restore(RoomState state)
    {
        if (state.Objects is not null)
        {
            _objects.Clear();

            foreach (var id in state.Objects)
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
