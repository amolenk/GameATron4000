namespace Amolenk.GameATron4000.Scripting.Model;

public class Room
{
    private readonly Game _game;
    private readonly List<GameObject> _objects;

    public string Id { get; }
    public Polygon WalkboxArea { get; }
    internal RoomActionHandlers ActionHandlers { get; private set; }
    public IReadOnlyList<GameObject> Objects => _objects.AsReadOnly();

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
        ActionHandlers = builder.ActionHandlers;
    }

    public void Enter()
    {
        if (ActionHandlers.HandleBeforeEnter is not null)
        {
            // Do not enqueue events while calling the BeforeEnter handler.
            // Any changes happening in the handler should not be made visible
            // in the UI yet.
            _game.EventQueue.IgnoreNewEvents = true;

            ActionHandlers.HandleBeforeEnter();

            _game.EventQueue.IgnoreNewEvents = false;
        }

        _game.EventQueue.Enqueue(new EnterRoomActionExecuted(this));

        _game.NotifyRoomEntered(this);
    }

    public void Put(GameObject gameObject, int x, int y)
    {
        if (gameObject.Room is not null)
        {
            gameObject.Room.Remove(gameObject);
        }

        gameObject.Position = new Point(x, y);

        Add(gameObject);

        // TODO Enqueue event
    }

    private void Add(GameObject gameObject)
    {
        gameObject.Room = this;
        _objects.Add(gameObject);
    }

    private void Remove(GameObject gameObject)
    {
        gameObject.Room = null;
        _objects.Remove(gameObject);
    }
}
