namespace Amolenk.GameATron4000.Model.Builders;

public abstract class GameObjectBuilder<TObject, TBuilder>
    where TObject : GameObject
    where TBuilder : GameObjectBuilder<TObject, TBuilder>
{
    private readonly TBuilder _builderInstance;

    protected string _id;
    protected Game _game;
    protected string _displayName;
    protected RelativePosition _interactPosition;
    protected string _interactStatus;
    protected bool _isTouchable;
    protected int _scrollFactor;
    protected int _depthOffset;
    protected string _status;

    public ActionHandlersBuilder<GameObjectBuilder<TObject, TBuilder>> When
    { 
        get;
    }

    protected GameObjectBuilder(string id, Game game)
    {
        _builderInstance = (TBuilder)this;

        _id = id;
        _game = game;
        _displayName = id;
        _interactPosition = RelativePosition.Center;
        _interactStatus = WellKnownStatus.FaceCamera;
        _isTouchable = true;
        _scrollFactor = -1;
        _depthOffset = 0;
        _status = WellKnownStatus.Default;

        When = new(this);
    }

    public TBuilder FixedToCamera()
    {
        _scrollFactor = 0;
        return _builderInstance;
    }

    public TBuilder Named(string displayName)
    {
        _displayName = displayName;
        return _builderInstance;
    }

    public TBuilder Untouchable()
    {
        _isTouchable = false;
        return _builderInstance;
    }

    public TBuilder WithActorInteraction(
        RelativePosition? position = null,
        string? status = null)
    {
        if (position.HasValue)
        {
            _interactPosition = position.Value;
        }

        if (status is not null)
        {
            _interactStatus = status;
        }

        return _builderInstance;
    }

    public TBuilder WithDepthOffset(int offset)
    {
        _depthOffset = offset;
        return _builderInstance;
    }

    public TBuilder WithStatus(string status)
    {
        _status = status;
        return _builderInstance;
    }

    public abstract TObject Build();

    protected ActionHandlers BuildActionHandlers() => new ActionHandlers(
        When.HandleGive,
        When.HandlePickUp,
        When.HandleUse,
        When.HandleOpen,
        When.HandleLookAt,
        When.HandlePush,
        When.HandleClose,
        When.HandleTalkTo,
        When.HandlePull,
        When.HandleWalkTo);
}
