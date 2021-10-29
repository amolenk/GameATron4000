namespace Amolenk.GameATron4000.Scripting.Model;

public class Room
{
    public string Id { get; }
    public Polygon Walkbox { get; }
    public IReadOnlyList<Actor> Actors => _actors.AsReadOnly();

    private readonly List<Actor> _actors;

    internal Room(
        string id,
        IEnumerable<Point> walkbox)
    {
        Id = id;
        Walkbox = new Polygon(walkbox);

        _actors = new List<Actor>();
    }

    internal void AddActor(Actor actor) => _actors.Add(actor);

    internal void RemoveActor(Actor actor) => _actors.Remove(actor);
}
