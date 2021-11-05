namespace Amolenk.GameATron4000.Scripting.Model;

public class Game
{
    private List<Actor> _actors;
    private List<Room> _rooms;
    private ICollector<IEvent> _events;
    private Action? _onStart;

    public Room? CurrentRoom { get; private set; }

    public Actor? Protagonist { get; private set; }

    internal Game(ICollector<IEvent> events)
    {
        _actors = new();
        _rooms = new();
        _events = events;
    }

    public ActorBuilder Actor(string id) =>
        new ActorBuilder(id, new Collector<Actor>(_actors), _events);

    public Room AddRoom(string id, IEnumerable<Point> walkbox)
    {
        var room = new Room(id, new Polygon(walkbox));
        _rooms.Add(room);
        return room;
    }

    public void EnterRoom(Room room)
    {
        CurrentRoom = room;

        _events.Add(new RoomEntered(CurrentRoom));
    }

    public void OnGameStart(Action onStart) => _onStart = onStart;

    public void PutActor(Actor actor, Room room, int x, int y)
    {
        if (actor.Room != null)
        {
            actor.Room.RemoveActor(actor);
        }

        actor.Position = new Point(x, y);
        actor.Room = room;

        room.AddActor(actor);
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