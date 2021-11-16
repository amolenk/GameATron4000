namespace Amolenk.GameATron4000.Model.State;

public abstract record GameObjectState
{
    public Point? Position { get; }

    public string? Status { get; }

    protected GameObjectState(Point? position, string? status)
    {
        Position = position;
        Status = status;
    }
}