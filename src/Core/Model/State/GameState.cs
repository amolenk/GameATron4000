namespace Amolenk.GameATron4000.Model.State;

public record GameState : IState<GameState>
{
    public Dictionary<string, ItemState> Items { get; }
    public Dictionary<string, ActorState> Actors { get; }
    public Dictionary<string, RoomState> Rooms { get; }
    public List<string>? Flags { get; }
    public string? Protagonist { get; }
    public string? CurrentRoom { get; }
    public string? PreviousRoom { get; }

    public GameState(
        Dictionary<string, ItemState> items,
        Dictionary<string, ActorState> actors,
        Dictionary<string, RoomState> rooms,
        List<string>? flags,
        string? protagonist,
        string? currentRoom,
        string? previousRoom)
    {
        Items = items;
        Actors = actors;
        Rooms = rooms;
        Flags = flags;
        Protagonist = protagonist;
        CurrentRoom = currentRoom;
        PreviousRoom = previousRoom;
    }

    public GameState GetChanges(GameState baseline)
    {
        var items = GetChanges(Items, baseline.Items);
        var actors = GetChanges(Actors, baseline.Actors);
        var rooms = GetChanges(Rooms, baseline.Rooms);
        var flags = HasFlagsChanges(baseline.Flags) ? Flags : null!;

        var protagonist = Protagonist != baseline.Protagonist
            ? Protagonist : null;

        var currentRoom = CurrentRoom != baseline.CurrentRoom
            ? CurrentRoom : null;

        var previousRoom = PreviousRoom != baseline.PreviousRoom
            ? PreviousRoom : null;

        return new GameState(
            items,
            actors,
            rooms,
            flags,
            protagonist,
            currentRoom,
            previousRoom);
    }

    private Dictionary<string, T> GetChanges<T>(
        Dictionary<string, T> first,
        Dictionary<string, T> second)
        where T : IState<T>
    {
        Dictionary<string, T> result = new();

        foreach (var firstItem in first)
        {
            if (second.TryGetValue(firstItem.Key, out T secondItem))
            {
                var changes = firstItem.Value.GetChanges(secondItem);
                if (changes is not null)
                {
                    result.Add(firstItem.Key, changes);
                }
            }
            else
            {
                result.Add(firstItem.Key, firstItem.Value);
            }
        }
        return result;
    }

    private bool HasFlagsChanges(List<string>? baseline) =>
        baseline is not null &&
        Flags is not null &&
        (Flags.Count != baseline.Count || !Flags.All(baseline.Contains));
}
