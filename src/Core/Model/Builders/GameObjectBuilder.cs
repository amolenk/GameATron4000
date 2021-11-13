namespace Amolenk.GameATron4000.Model.Builders;

public abstract class GameObjectBuilder<TObject, TBuilder>
    where TObject : GameObject
    where TBuilder : GameObjectBuilder<TObject, TBuilder>
{
    private readonly TBuilder _builderInstance;

    protected string _id;
    protected string _displayName;
    protected string _frame;
    protected Game _game;
    protected string _interactFrame;
    protected RelativePosition _interactPosition;
    protected bool _isTouchable;
    protected int _scrollFactor;

    public ActionHandlersBuilder<GameObjectBuilder<TObject, TBuilder>> When
    { 
        get;
    }

    protected GameObjectBuilder(string id, Game game)
    {
        _builderInstance = (TBuilder)this;

        _id = id;
        _displayName = id;
        _frame = WellKnownFrame.Default;
        _game = game;
        _interactFrame = WellKnownFrame.Default;
        _interactPosition = RelativePosition.Center;
        _isTouchable = true;
        _scrollFactor = -1;

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
        RelativePosition position,
        string frame)
    {
        _interactPosition = position;
        _interactFrame = frame;
        return _builderInstance;
    }

    public TBuilder WithFrame(string frame)
    {
        _frame = frame;
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
        When.HandlePull);
}
