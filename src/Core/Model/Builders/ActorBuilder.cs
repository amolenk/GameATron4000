namespace Amolenk.GameATron4000.Model.Builders;

public class ActorBuilder : IGameObjectBuilder
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
    public string TextColor { get; private set; }
    public GameObjectHandlersBuilder<ActorBuilder> When { get; private set; }
    public Game Game { get; private set; }
    public GameObjectState State { get; private set; }

    IGameObjectHandlersBuilder IGameObjectBuilder.HandlersBuilder => When;

    internal ActorBuilder(string id, Game game)
    {
        Id = id;
        DisplayName = id;
        IsTouchable = true;
        UseWith = false;
        FrameName = WellKnownFrame.FaceCamera;
        InteractFrameName = WellKnownFrame.FaceCamera;
        InteractPosition = RelativePosition.Center;
        ScrollFactor = -1;
        TextColor = "white";
        When = new(this);
        Game = game;
    }

    public ActorBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return this;
    }

    public ActorBuilder Untouchable()
    {
        IsTouchable = false;
        return this;
    }

    public ActorBuilder CanBeUsedWithOtherObjects()
    {
        UseWith = true;
        return this;
    }

    public ActorBuilder WithFrame(string frameName)
    {
        FrameName = frameName;
        return this;
    }

    public ActorBuilder WithActorInteraction(
        RelativePosition position,
        string frameName)
    {
        InteractFrameName = frameName;
        InteractPosition = position;
        return this;
    }

    public ActorBuilder DependsOn(GameObject other, string frameName)
    {
        Condition = new GameObjectCondition(other, frameName);
        return this;
    }

    public ActorBuilder FixedToCamera()
    {
        ScrollFactor = 0;
        return this;
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        TextColor = textColor;
        return this;
    }

    public Actor Build() => new Actor(this);
}
