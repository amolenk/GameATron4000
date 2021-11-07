namespace Amolenk.GameATron4000.Scripting.Model;

public class GameObject
{
    public string Id { get; }
    public string DisplayName { get; }
    public string State { get; }
    public RelativePosition? InteractPosition { get; }
    public string? InteractState { get; }
    public bool IsTouchable { get; }
    public int ScrollFactor { get; }
    public bool IsVisible => State != WellKnownState.Invisible;
    public Point Position { get; internal set; } = new Point(0, 0);
    public Room? Room { get; internal set; }
    // TODO NULL???
    internal ActionHandlers? ActionHandlers { get; private set; }
    protected ICollector<IEvent>? Events { get; private set; }

    internal GameObject(IGameObjectBuilder builder)
    {
        Id = builder.Id;
        DisplayName = builder.DisplayName;
        State = builder.State;
        IsTouchable = builder.IsTouchable;
        ScrollFactor = builder.ScrollFactor;
        InteractPosition = builder.InteractPosition;
        InteractState = builder.InteractState;
        ActionHandlers = builder.ActionHandlers;
        Events = builder.Events;
    }
}
