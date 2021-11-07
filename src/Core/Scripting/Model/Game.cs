namespace Amolenk.GameATron4000.Scripting.Model;

public class Game
{
    private List<GameObject> _objects;
    private List<Room> _rooms;
    private Action? _onStart;

    public Actor? Protagonist { get; private set; }
    public Room? CurrentRoom { get; private set; }

    internal ActionEventQueue EventQueue { get; private set; }

    internal Game(ActionEventQueue eventQueue)
    {
        _objects = new();
        _rooms = new();

        EventQueue = eventQueue;
    }

    public ActorBuilder Actor(string id) =>
        new ActorBuilder(id, this);

    public GameObjectBuilder Object(string id) =>
        new GameObjectBuilder(id, this);

    public RoomBuilder Room(string id) =>
        new RoomBuilder(id, this);

    public void OnGameStart(Action onStart) => _onStart = onStart;

    // public void PutObject(GameObject gameObject, Room room, int x, int y)
    // {
    //     if (gameObject.Room != null)
    //     {
    //         gameObject.Room.RemoveObject(gameObject);
    //     }

    //     gameObject.Position = new Point(x, y);
    //     gameObject.Room = room;

    //     room.AddObject(gameObject);
    // }

    public void SayLine(string line) => Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        Protagonist = actor;

        EventQueue.Enqueue(new ProtagonistChanged(actor));
    }

    internal void Start()
    {
        if (_onStart is not null)
        {
            _onStart();
        }
    }

    // internal void NotifyRoomAdded(Room room)
    // {
    //     _rooms.Add(room);
    // }

    internal void NotifyRoomEntered(Room room)
    {
        CurrentRoom = room;
    }
}