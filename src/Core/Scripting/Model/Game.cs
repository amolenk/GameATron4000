namespace Amolenk.GameATron4000.Scripting.Model;

public class Game
{
    private List<Actor> _actors;
    private List<Room> _rooms;
    private NotificationCollector _notificationCollector;
    private Action? _onStart;

    public Room? CurrentRoom { get; private set; }

    public Actor? Protagonist { get; private set; }

    internal Game(NotificationCollector notificationCollector)
    {
        _actors = new();
        _rooms = new();
        _notificationCollector = notificationCollector;
    }

    public ActorBuilder Actor(string id)
    {
        return new ActorBuilder(id, new ItemCollector<Actor>(_actors));
    }

    public Room AddRoom(string id, IEnumerable<Point> walkbox)
    {
        var room = new Room(id, new Polygon(walkbox));
        _rooms.Add(room);
        return room;
    }

    public void EnterRoom(Room room)
    {
        CurrentRoom = room;

        _notificationCollector.Add(new RoomEntered(CurrentRoom));
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

    public void SetProtagonist(Actor actor)
    {
        Protagonist = actor;

        _notificationCollector.Add(new ProtagonistChanged(actor));
    }

    internal void Start()
    {
        // TODO Is this null check the way to go?
        if (_onStart is not null)
        {
            _onStart();
        }
    }
}