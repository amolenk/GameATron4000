namespace Amolenk.GameATron4000.Scripting.Model;

public class Game
{
    private List<GameObject> _objects;
    private List<Room> _rooms;
    private ICollector<IEvent> _events;
    private Action? _onStart;

    public Room? CurrentRoom { get; private set; }

    public Actor? Protagonist { get; private set; }

    internal Game(ICollector<IEvent> events)
    {
        _objects = new();
        _rooms = new();
        _events = events;
    }

    public ActorBuilder Actor(string id) =>
        new ActorBuilder(id, _events, new Collector<GameObject>(_objects));

    public GameObjectBuilder Object(string id) =>
        new GameObjectBuilder(id, _events, new Collector<GameObject>(_objects));

    public RoomBuilder Room(string id) =>
        new RoomBuilder(id, new Collector<Room>(_rooms));

    public void EnterRoom(Room room)
    {
        CurrentRoom = room;

        _events.Add(new EnterRoomActionExecuted(CurrentRoom));
    }

    public void OnGameStart(Action onStart) => _onStart = onStart;

    public void PutObject(GameObject gameObject, Room room, int x, int y)
    {
        if (gameObject.Room != null)
        {
            gameObject.Room.RemoveObject(gameObject);
        }

        gameObject.Position = new Point(x, y);
        gameObject.Room = room;

        room.AddObject(gameObject);
    }

    public void SayLine(string line) => Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        Protagonist = actor;

        _events.Add(new ProtagonistChanged(actor));
    }

    internal void Start()
    {
        if (_onStart is not null)
        {
            _onStart();
        }
    }
}