namespace Amolenk.GameATron4000.Scripting.Model;

public class Actor : GameObject
{
    public string DisplayName { get; }
    public string TextColor { get; }
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
        RelativePosition interactPosition,
        Direction interactDirection,
        bool isTouchable,
        bool isVisible) : base(id)
    {
        DisplayName = displayName;
        TextColor = textColor;
        InteractPosition = interactPosition;
        InteractDirection = interactDirection;
        IsTouchable = isTouchable;
        IsVisible = isVisible;
    }
}
