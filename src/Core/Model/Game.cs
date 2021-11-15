namespace Amolenk.GameATron4000.Model;

public class Game
{
    private readonly List<Item> _items;
    private readonly List<Actor> _actors;
    private readonly List<Room> _rooms;
    private readonly List<string> _flags;
    private Action? _onStart;

    public Actor? Protagonist { get; private set; }
    public Room? CurrentRoom { get; private set; }
    public Room? PreviousRoom { get; private set; }

    internal EventQueue EventQueue { get; private set; }

    internal Game(EventQueue eventQueue)
    {
        _items = new();
        _actors = new();
        _rooms = new();
        _flags = new();

        EventQueue = eventQueue;
    }

    public Item AddItem(string id, Action<ItemBuilder> configure)
    {
        ItemBuilder builder = new(id, this);
        configure(builder);

        var item = builder.Build();

        _items.Add(item);

        return item;
    }

    public Actor AddActor(string id, Action<ActorBuilder> configure)
    {
        ActorBuilder builder = new(id, this);
        configure(builder);

        var actor = builder.Build();

        _actors.Add(actor);

        return actor;
    }

    public Room AddRoom(string id, Action<RoomBuilder> configure)
    {
        RoomBuilder builder = new(id, this);
        configure(builder);

        var room = builder.Build();

        _rooms.Add(room);

        return room;
    }

    //int Random(int min, int max);

    public void ChangeRoom(Room room)
    {
        if (room != CurrentRoom)
        {
            PreviousRoom = CurrentRoom;
            CurrentRoom = room;

            room.Enter();
        }
    }

    public void OnGameStart(Action onStart) => _onStart = onStart;

    public void Delay(int value) =>
        EventQueue.Enqueue(new DelayRequested(TimeSpan.FromMilliseconds(value)));

    // TODO Remove
    public void SayLine(string line) => Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        Protagonist = actor;

        EventQueue.Enqueue(new ProtagonistChanged(actor, new List<Item>()));
    }

    public void SetFlag(string flag)
    {
        if (!_flags.Contains(flag))
        {
            _flags.Add(flag);
        }
    }

    public void ClearFlag(string flag) => _flags.Remove(flag);

    public bool IsFlagSet(string flag) => _flags.Contains(flag);

    public void StartDialogue(string dialogueName)
    {
        SayLine($"[TODO: {dialogueName}]");
    }

    internal bool TryGetItem(
        string id, 
        [MaybeNullWhen(false)] out Item item)
    {
        item = _items.FirstOrDefault(item => item.Id == id);
        return item is not null;
    }

    internal bool TryGetActor(
        string id, 
        [MaybeNullWhen(false)] out Actor actor)
    {
        actor = _actors.FirstOrDefault(item => item.Id == id);
        return actor is not null;
    }

    internal bool TryGetRoomForObject(
        IGameObject gameObject,
        [MaybeNullWhen(false)] out Room room)
    {
        room = _rooms.FirstOrDefault(room => room.ContainsObject(gameObject));
        return room is not null;
    }

    internal bool TryGetOwnerForItem(
        Item item,
        [MaybeNullWhen(false)] out Actor actor)
    {
        actor = _actors.FirstOrDefault(actor => actor.Has(item));
        return actor is not null;
    }

    internal void Start() => _onStart?.Invoke();

    internal GameSnapshot Save()
    {
        var items = _items.ToDictionary(
            item => item.Id,
            item => item.Save());

        var actors = _actors.ToDictionary(
            actor => actor.Id,
            actor => actor.Save());

        var rooms = _rooms.ToDictionary(
            room => room.Id,
            room => room.Save());

        var protagonist = Protagonist?.Id;
        var currentRoom = CurrentRoom?.Id;
        var previousRoom = PreviousRoom?.Id;

        return new GameSnapshot(
            items,
            actors,
            rooms,
            new List<string>(_flags),
            protagonist,
            currentRoom,
            previousRoom);
    }

    internal void Restore(GameSnapshot snapshot)
    {
        foreach (var entry in snapshot.Items)
        {
            var item = _items.FirstOrDefault(item => item.Id == entry.Key);
            item?.Restore(entry.Value);
        }

        foreach (var entry in snapshot.Actors)
        {
            var actor = _actors.FirstOrDefault(actor => actor.Id == entry.Key);
            actor?.Restore(entry.Value);
        }

        foreach (var entry in snapshot.Rooms)
        {
            var room = _rooms.FirstOrDefault(room => room.Id == entry.Key);
            room?.Restore(entry.Value);
        }

        if (snapshot.Protagonist is not null)
        {
            var protagonist = _actors.FirstOrDefault(
                actor => actor.Id == snapshot.Protagonist);
            
            if (protagonist is not null)
            {
                Protagonist = protagonist;
            }
        }

        if (snapshot.CurrentRoom is not null)
        {
            var currentRoom = _rooms.FirstOrDefault(
                room => room.Id == snapshot.CurrentRoom);
            
            if (currentRoom is not null)
            {
                CurrentRoom = currentRoom;
            }
        }

        if (snapshot.PreviousRoom is not null)
        {
            var previousRoom = _rooms.FirstOrDefault(
                room => room.Id == snapshot.CurrentRoom);
            
            if (previousRoom is not null)
            {
                PreviousRoom = previousRoom;
            }
        }
    }
}
