namespace Amolenk.GameATron4000.Model;

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
    public Actor? Owner { get; internal set; }
    internal GameObjectHandlers Handlers { get; private set; }
    protected Game Game { get; private set; }

    internal GameObject(IGameObjectBuilder builder)
    {
        Id = builder.Id;
        DisplayName = builder.DisplayName;
        State = builder.State;
        IsTouchable = builder.IsTouchable;
        ScrollFactor = builder.ScrollFactor;
        InteractPosition = builder.InteractPosition;
        InteractState = builder.InteractState;
        Handlers = new(builder.When);
        Game = builder.Game;
    }

    public override bool Equals(object? obj)
    {
        if (obj is GameObject gameObject)
        {
            return Id.Equals(gameObject.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
