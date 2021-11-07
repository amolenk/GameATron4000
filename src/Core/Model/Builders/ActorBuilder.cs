namespace Amolenk.GameATron4000.Model.Builders;

public class ActorBuilder : IGameObjectBuilder
{
    public string Id { get; private set; }
    public string DisplayName { get; protected set; }
    public string State { get; protected set; }
    public bool IsTouchable { get; protected set; }
    public int ScrollFactor { get; protected set; }
    public RelativePosition InteractPosition { get; protected set; }
    public string InteractState { get; protected set; }
    public string TextColor { get; private set; }
    public GameObjectHandlersBuilder<ActorBuilder> When { get; protected set; }
    public Game Game { get; private set; }

    IGameObjectHandlersBuilder IGameObjectBuilder.When => When;

    internal ActorBuilder(string id, Game game)
    {
        Id = id;
        DisplayName = id;
        State = WellKnownState.FaceCamera;
        IsTouchable = true;
        ScrollFactor = -1;
        InteractPosition = RelativePosition.Center;
        InteractState = WellKnownState.Default;
        When = new(this);
        Game = game;
    }

    public ActorBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public ActorBuilder InState(string state)
    {
        State = state;
        return this;
    }

    public ActorBuilder Untouchable()
    {
        IsTouchable = false;
        return this;
    }

    public ActorBuilder Invisible()
    {
        State = WellKnownState.Invisible;
        return this;
    }

    public ActorBuilder WithScrollFactor(int scrollFactor)
    {
        ScrollFactor = scrollFactor;
        return this;
    }

    public ActorBuilder InteractFromPosition(RelativePosition position)
    {
        InteractPosition = position;
        return this;
    }

    public ActorBuilder InteractInState(string state)
    {
        InteractState = state;
        return this;
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        TextColor = textColor;
        return this;
    }

    public Actor Build() => new Actor(this);
}
