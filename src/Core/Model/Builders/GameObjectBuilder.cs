namespace Amolenk.GameATron4000.Model.Builders;

public abstract class GameObjectBuilder<TObject, TBuilder> : IGameObjectBuilder
    where TObject : GameObject
    where TBuilder : GameObjectBuilder<TObject, TBuilder>
{
    private readonly TBuilder _builderInstance;

    public string DisplayName { get; private set; }
    public RelativePosition InteractPosition { get; private set; }
    public string InteractStatus { get; private set; }
    public bool IsTouchable { get; private set; }
    public int ScrollFactor { get; private set; }
    public int DepthOffset { get; private set; }
    public string Status { get; protected set; }
    public ActionHandlersBuilder When { get; }

    protected GameObjectBuilder()
    {
        _builderInstance = (TBuilder)this;

        DisplayName = string.Empty;
        InteractPosition = RelativePosition.Center;
        InteractStatus = WellKnownStatus.FaceCamera;
        IsTouchable = true;
        ScrollFactor = -1;
        DepthOffset = 0;
        Status = WellKnownStatus.Default;
        When = new();
    }

    public TBuilder FixedToCamera()
    {
        ScrollFactor = 0;
        return _builderInstance;
    }

    public TBuilder Named(string displayName)
    {
        DisplayName = displayName;
        return _builderInstance;
    }

    public TBuilder Untouchable()
    {
        IsTouchable = false;
        return _builderInstance;
    }

    public TBuilder WithActorInteraction(
        RelativePosition? position = null,
        string? status = null)
    {
        if (position.HasValue)
        {
            InteractPosition = position.Value;
        }

        if (status is not null)
        {
            InteractStatus = status;
        }

        return _builderInstance;
    }

    public TBuilder WithDepthOffset(int offset)
    {
        DepthOffset = offset;
        return _builderInstance;
    }

    public TBuilder WithStatus(string status)
    {
        Status = status;
        return _builderInstance;
    }
}
