namespace Amolenk.GameATron4000.Scripting.Model;

public class Room
{
    public string Id { get; }
    public Polygon WalkboxArea { get; }
    public IReadOnlyList<GameObject> Objects => _objects.AsReadOnly();

    private readonly List<GameObject> _objects;

    internal Room(RoomBuilder builder)
    {
        if (builder.WalkboxArea is null)
        {
            throw new ArgumentException(
                "Walkbox area must be set for a room.",
                nameof(builder));
        }

        _objects = new List<GameObject>();

        Id = builder.Id;
        WalkboxArea = builder.WalkboxArea;
    }

    internal void AddObject(GameObject gameObject) => _objects.Add(gameObject);

    internal void RemoveObject(GameObject gameObject) =>
        _objects.Remove(gameObject);
}
