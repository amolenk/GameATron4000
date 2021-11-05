namespace Amolenk.GameATron4000.Scripting.Model;

public class ActorBuilder
{
    private readonly string _id;
    private readonly ICollector<Actor> _actors;
    private readonly ICollector<IEvent> _events;
    private string _displayName;
    private string _textColor;
    private Direction _facingDirection;
    private RelativePosition _interactPosition;
    private Direction _interactDirection;
    private bool _isTouchable;
    private bool _isVisible;
    private ActionHandlers _actionHandlers;

    internal ActorBuilder(
        string id,
        ICollector<Actor> actors,
        ICollector<IEvent> events)
    {
        _id = id;
        _actors = actors;
        _events = events;
        _displayName = id;
        _textColor = "White";
        _facingDirection = Direction.Front;
        _interactPosition = RelativePosition.None;
        _interactDirection = Direction.Front;
        _isTouchable = true;
        _isVisible = true;
        _actionHandlers = new();
    }

    public ActorBuilder Named(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        _textColor = textColor;
        return this;
    }

    public ActorBuilder FacingDirection(Direction direction)
    {
        _facingDirection = direction;
        return this;
    }

    public ActorBuilder InteractFromPosition(RelativePosition position)
    {
        _interactPosition = position;
        return this;
    }

    public ActorBuilder InteractWhileFacing(Direction direction)
    {
        _interactDirection = direction;
        return this;
    }

    public ActorBuilder Untouchable()
    {
        _isTouchable = false;
        return this;
    }

    public ActorBuilder Invisible()
    {
        _isVisible = false;
        return this;
    }

    public ActorBuilder When(Action<ActionHandlers> configure)
    {
        configure(_actionHandlers);
        return this;
    }

    public Actor Add()
    {
        var actor = new Actor(
            _id,
            _displayName,
            _textColor,
            _facingDirection,
            _interactPosition,
            _interactDirection,
            _isTouchable,
            _isVisible,
            _actionHandlers,
            _events);

        _actors.Add(actor);

        return actor;
    }
}