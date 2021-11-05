namespace Amolenk.GameATron4000.Scripting.Model;

public class Actor : GameObject
{
    private readonly ICollector<IEvent> _events;

    public string DisplayName { get; }
    public string TextColor { get; }
    public Direction FacingDirection { get; }
    public RelativePosition InteractPosition { get; }
    public Direction InteractDirection { get; }
    public bool IsTouchable { get; }
    public bool IsVisible { get; }
    public Point Position { get; internal set; } = new Point(0, 0);
    public Room? Room { get; internal set; }

    internal Actor(
        string id,
        string displayName,
        string textColor,
        Direction facingDirection,
        RelativePosition interactPosition,
        Direction interactDirection,
        bool isTouchable,
        bool isVisible,
        ActionHandlers actionHandlers,
        ICollector<IEvent> events)
        : base(id, actionHandlers)
    {
        _events = events;

        DisplayName = displayName;
        TextColor = textColor;
        FacingDirection = facingDirection;
        InteractPosition = interactPosition;
        InteractDirection = interactDirection;
        IsTouchable = isTouchable;
        IsVisible = isVisible;
    }

    public void SayLine(string line)
    {
        _events.Add(new LineSpoken(this, line));
    }
}
