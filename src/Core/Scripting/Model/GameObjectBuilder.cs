namespace Amolenk.GameATron4000.Scripting.Model;

public abstract class GameObjectBuilder<TObject, TBuilder> : IGameObjectBuilder
    where TObject : GameObject
    where TBuilder : GameObjectBuilder<TObject, TBuilder>
{
    private readonly ICollector<GameObject> _gameObjects;

    public string Id { get; private set; }
    public string DisplayName { get; protected set; }
    public string State { get; protected set; }
    public bool IsTouchable { get; protected set; }
    public int ScrollFactor { get; protected set; }
    public RelativePosition InteractPosition { get; protected set; }
    public string InteractState { get; protected set; }
    public ActionHandlers ActionHandlers { get; protected set; }
    public ICollector<IEvent> Events { get; private set; }

    internal GameObjectBuilder(
        string id,
        ICollector<IEvent> events,
        ICollector<GameObject> gameObjects)
    {
        _gameObjects = gameObjects;

        Id = id;
        DisplayName = id;
        State = WellKnownState.Default;
        IsTouchable = true;
        ScrollFactor = -1;
        InteractPosition = RelativePosition.None;
        InteractState = WellKnownState.Default;
        ActionHandlers = new();
        Events = events;
    }

    public TBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return (TBuilder)this;
    }

    public TBuilder InState(string state)
    {
        State = state;
        return (TBuilder)this;
    }

    public TBuilder Untouchable()
    {
        IsTouchable = false;
        return (TBuilder)this;
    }

    public TBuilder Invisible()
    {
        State = WellKnownState.Invisible;
        return (TBuilder)this;
    }

    public TBuilder WithScrollFactor(int scrollFactor)
    {
        ScrollFactor = scrollFactor;
        return (TBuilder)this;
    }

    public TBuilder InteractFromPosition(RelativePosition position)
    {
        InteractPosition = position;
        return (TBuilder)this;
    }

    public TBuilder InteractInState(string state)
    {
        InteractState = state;
        return (TBuilder)this;
    }

    public TBuilder When(Action<ActionHandlers> configure)
    {
        configure(ActionHandlers);
        return (TBuilder)this;
    }

    public TObject Add()
    {
        var gameObject = Build();

        _gameObjects.Add(gameObject);

        return gameObject;
    }

    protected abstract TObject Build();
}

public class GameObjectBuilder : GameObjectBuilder<GameObject, GameObjectBuilder>
{
    public GameObjectBuilder(
        string id,
        ICollector<IEvent> events,
        ICollector<GameObject> gameObjects)
        : base(id, events, gameObjects)
    {
    }

    protected override GameObject Build() => new GameObject(this);
}