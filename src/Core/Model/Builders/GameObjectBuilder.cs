namespace Amolenk.GameATron4000.Model.Builders;

public class GameObjectBuilder : IGameObjectBuilder
{
    public string Id { get; private set; }
    public string DisplayName { get; protected set; }
    public string State { get; protected set; }
    public bool IsTouchable { get; protected set; }
    public int ScrollFactor { get; protected set; }
    public RelativePosition InteractPosition { get; protected set; }
    public string InteractState { get; protected set; }
    public GameObjectHandlersBuilder<GameObjectBuilder> When { get; protected set; }
    public Game Game { get; private set; }

    IGameObjectHandlersBuilder IGameObjectBuilder.When => When;

    internal GameObjectBuilder(string id, Game game)
    {
        Id = id;
        DisplayName = id;
        State = WellKnownState.Default;
        IsTouchable = true;
        ScrollFactor = -1;
        InteractPosition = RelativePosition.Center;
        InteractState = WellKnownState.Default;
        When = new(this);
        Game = game;
    }

    public GameObjectBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public GameObjectBuilder InState(string state)
    {
        State = state;
        return this;
    }

    public GameObjectBuilder Untouchable()
    {
        IsTouchable = false;
        return this;
    }

    public GameObjectBuilder Invisible()
    {
        State = WellKnownState.Invisible;
        return this;
    }

    public GameObjectBuilder WithScrollFactor(int scrollFactor)
    {
        ScrollFactor = scrollFactor;
        return this;
    }

    public GameObjectBuilder InteractFromPosition(RelativePosition position)
    {
        InteractPosition = position;
        return this;
    }

    public GameObjectBuilder InteractInState(string state)
    {
        InteractState = state;
        return this;
    }

    public GameObject Build() => new GameObject(this);
}
