namespace Amolenk.GameATron4000.Messages.Events;

// Because the model may be further ahead than the UI rendering, we need to
// include the original position/status in some events.
public record GameObjectSnapshot : IEvent
{
    public GameObject GameObject { get; }
    public Point Position { get; }
    public string Status { get; }

    public GameObjectSnapshot(GameObject gameObject)
    {
        GameObject = gameObject;
        Position = gameObject.Position;
        Status = gameObject.Status;
    }
}
