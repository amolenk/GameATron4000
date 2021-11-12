namespace Amolenk.GameATron4000.Model.Builders;

public class GameObjectBuilder : IGameObjectBuilder
{
    public string Id { get; private set; }
    public string DisplayName { get; private set; }
    public bool IsTouchable { get; private set; }
    public bool UseWith { get; private set; }
    public string FrameName { get; private set; }
    public string InteractFrameName { get; private set; }
    public RelativePosition InteractPosition { get; private set; }
    public GameObjectCondition? Condition { get; private set; }
    public int ScrollFactor { get; private set; }
    public GameObjectHandlersBuilder<GameObjectBuilder> When { get; private set; }
    public Game Game { get; private set; }

    IGameObjectHandlersBuilder IGameObjectBuilder.HandlersBuilder => When;

    internal GameObjectBuilder(string id, Game game)
    {
        Id = id;
        DisplayName = id;
        IsTouchable = true;
        UseWith = false;
        FrameName = WellKnownFrame.Default;
        InteractFrameName = WellKnownFrame.Default;
        InteractPosition = RelativePosition.Center;
        ScrollFactor = -1;
        When = new(this);
        Game = game;
    }

    public GameObjectBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public GameObjectBuilder Untouchable()
    {
        IsTouchable = false;
        return this;
    }

    public GameObjectBuilder CanBeUsedWithOtherObjects()
    {
        UseWith = true;
        return this;
    }

    public GameObjectBuilder WithFrameName(string frameName)
    {
        FrameName = frameName;
        return this;
    }

    public GameObjectBuilder WithActorInteraction(
        RelativePosition position,
        string frameName)
    {
        InteractFrameName = frameName;
        InteractPosition = position;
        return this;
    }

    public GameObjectBuilder DependsOn(GameObject other, string frameName)
    {
        Condition = new GameObjectCondition(other, frameName);
        return this;
    }

    public GameObjectBuilder FixedToCamera()
    {
        ScrollFactor = 0;
        return this;
    }

    public GameObject Build() => new GameObject(this);
}
