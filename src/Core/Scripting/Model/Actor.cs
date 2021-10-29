namespace Amolenk.GameATron4000.Scripting.Model;

public class Actor
{
    public string Id { get; }

    public Point Position { get; internal set; } = new Point(0, 0);

    public Room? Room { get; internal set; }

    internal Actor(string id)
    {
        Id = id;
    }
}
