namespace Amolenk.GameATron4000.Model;

public class Game
{
    // TODO
    private readonly GlobalState _state;
    private Action? _onStart;

    public Actor? Protagonist => _state.Protagonist;

    public Room? CurrentRoom => _state.CurrentRoom;

    internal EventQueue EventQueue { get; private set; }

    internal Game(EventQueue eventQueue)
    {
        _state = new();

        EventQueue = eventQueue;
    }

    public Item AddItem(string id, Action<ItemBuilder> configure)
    {
        ItemBuilder builder = new(id, this);
        configure(builder);

        var item = builder.Build();

        _state.Items.Add(item);

        return item;
    }

    public Actor AddActor(string id, Action<ActorBuilder> configure)
    {
        ActorBuilder builder = new(id, this);
        configure(builder);

        var actor = builder.Build();

        _state.Actors.Add(actor);

        return actor;
    }

    public Room AddRoom(string id, Action<RoomBuilder> configure)
    {
        RoomBuilder builder = new(id, this);
        configure(builder);

        var room = builder.Build();

        _state.Rooms.Add(room);

        return room;
    }

    //int Random(int min, int max);

    public void ChangeRoom(Room room)
    {
        if (room != _state.CurrentRoom)
        {
            room.Enter();

            _state.PreviousRoom = _state.CurrentRoom;
            _state.CurrentRoom = room;
        }
    }

    public void OnGameStart(Action onStart) => _onStart = onStart;

    public void Delay(int value) =>
        EventQueue.Enqueue(new DelayRequested(TimeSpan.FromMilliseconds(value)));

    public void SayLine(string line) => Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        _state.Protagonist = actor;

        EventQueue.Enqueue(new ProtagonistChanged(actor, new List<Item>()));
    }

    internal bool TryGetRoomForObject(
        GameObject gameObject,
        [MaybeNullWhen(false)] out Room room)
    {
        room = _state.Rooms.FirstOrDefault(
            room => room.Objects.Contains(gameObject));
        
        return room is not null;
    }

    internal bool TryGetOwnerForItem(
        Item item,
        [MaybeNullWhen(false)] out Actor actor)
    {
        actor = _state.Actors.FirstOrDefault(
            actor => actor.Inventory.Contains(item));
        
        return actor is not null;
    }

    internal void Start() => _onStart?.Invoke();



}
