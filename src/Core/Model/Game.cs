namespace Amolenk.GameATron4000.Model;

public class Game
{
    private readonly List<Item> _items;
    private readonly List<Actor> _actors;
    private readonly List<Room> _rooms;
    private readonly List<string> _flags;
    private readonly List<string> _cannedResponses;
    private readonly Random _random;
    private Action? _onStart;
    private DialogueTree? _activeDialogueTree;

    public Actor? Protagonist { get; private set; }
    public Room? CurrentRoom { get; private set; }
    public Room? PreviousRoom { get; private set; }
    public bool DialogueTreeActive => _activeDialogueTree is not null;

    internal EventQueue EventQueue { get; private set; }

    public Game(EventQueue eventQueue)
    {
        _items = new();
        _actors = new();
        _rooms = new();
        _flags = new();
        _cannedResponses = new();
        _random = new();

        EventQueue = eventQueue;
    }

    public Item AddItem(string id, Action<ItemBuilder>? configure = null)
    {
        ItemBuilder builder = new(id, this);
        if (configure is not null)
        {
            configure(builder);
        }

        var item = builder.Build();

        _items.Add(item);

        return item;
    }

    public Actor AddActor(string id, Action<ActorBuilder>? configure = null)
    {
        ActorBuilder builder = new(id, this);
        if (configure is not null)
        {
            configure(builder);
        }

        var actor = builder.Build();

        _actors.Add(actor);

        return actor;
    }

    public Room AddRoom(string id, Action<RoomBuilder>? configure = null)
    {
        RoomBuilder builder = new(id, this);
        if (configure is not null)
        {
            configure(builder);
        }

        var room = builder.Build();

        _rooms.Add(room);

        return room;
    }

    public DialogueTree AddDialogueTree(
        string id,
        Action<DialogueTreeBuilder> configure)
    {
        DialogueTreeBuilder builder = new(id);
        configure(builder);

        return builder.Build();
    }

    public void AddCannedResponse(string response)
    {
        _cannedResponses.Add(response);
    }

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

    public string GetCannedResponse()
    {
        if (_cannedResponses.Count > 0)
        {
            return _cannedResponses[_random.Next(0, _cannedResponses.Count)];
        }
        return "...";
    }

    // TODO Remove
    public void SayLine(string line) => Protagonist?.SayLine(line);

    public void SetProtagonist(Actor actor)
    {
        Protagonist = actor;

        EventQueue.Enqueue(new ProtagonistChanged(actor));
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

    public void StartDialogue(DialogueTree dialogueTree)
    {
        if (_activeDialogueTree is null)
        {
            var options = dialogueTree.Start().ToList();
            if (options.Any())
            {
                _activeDialogueTree = dialogueTree;
                EventQueue.Enqueue(new DialogueOptionsAvailable(options));
            }
            else
            {
                _activeDialogueTree = null;
            }
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot start a new dialogue while dialogue tree {_activeDialogueTree.Id} is still active.");
        }
    }

    internal void ContinueDialogue(DialogueOption option)
    {
        if (_activeDialogueTree is not null)
        {
            var options = _activeDialogueTree.Continue(option.Topic).ToList();
            if (options.Any())
            {
                EventQueue.Enqueue(new DialogueOptionsAvailable(options));
            }
            else
            {
                _activeDialogueTree = null;
            }
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot continue dialogue; no active dialogue tree found.");
        }
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
        GameObject gameObject,
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

    internal void Start()
    {
        _onStart?.Invoke();

        EventQueue.Enqueue(new GameStarted(this));
    } 

    internal GameState Save()
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

        return new GameState(
            items,
            actors,
            rooms,
            new List<string>(_flags),
            protagonist,
            currentRoom,
            previousRoom);
    }

    internal void Restore(GameState gameState)
    {
        foreach (var entry in gameState.Items)
        {
            var item = _items.FirstOrDefault(item => item.Id == entry.Key);
            item?.Restore(entry.Value);
        }

        foreach (var entry in gameState.Actors)
        {
            var actor = _actors.FirstOrDefault(actor => actor.Id == entry.Key);
            actor?.Restore(entry.Value);
        }

        foreach (var entry in gameState.Rooms)
        {
            var room = _rooms.FirstOrDefault(room => room.Id == entry.Key);
            room?.Restore(entry.Value);
        }

        if (gameState.Flags is not null)
        {
            _flags.Clear();
            _flags.AddRange(gameState.Flags);
        }

        if (gameState.Protagonist is not null)
        {
            var protagonist = _actors.FirstOrDefault(
                actor => actor.Id == gameState.Protagonist);
            
            if (protagonist is not null)
            {
                Protagonist = protagonist;
            }
        }

        if (gameState.CurrentRoom is not null)
        {
            var currentRoom = _rooms.FirstOrDefault(
                room => room.Id == gameState.CurrentRoom);
            
            if (currentRoom is not null)
            {
                CurrentRoom = currentRoom;
            }
        }

        if (gameState.PreviousRoom is not null)
        {
            var previousRoom = _rooms.FirstOrDefault(
                room => room.Id == gameState.CurrentRoom);
            
            if (previousRoom is not null)
            {
                PreviousRoom = previousRoom;
            }
        }
    }
}
