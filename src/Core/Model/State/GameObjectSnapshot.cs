namespace Amolenk.GameATron4000.Model.State;

public abstract record GameObjectSnapshot
{
    public Point? Position { get; }

    public string? Status { get; }

    protected GameObjectSnapshot(Point? position, string? status)
    {
        Position = position;
        Status = status;
    }
}