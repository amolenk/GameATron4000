namespace Amolenk.GameATron4000.Scripting.Model;

public class ActorBuilder
{
    private readonly string _id;
    private readonly ItemCollector<Actor> _actorCollector;
    private string _displayName;
    private string _textColor;
    private RelativePosition _interactPosition;
    private Direction _interactDirection;
    private bool _isTouchable;
    private bool _isVisible;

    internal ActorBuilder(string id, ItemCollector<Actor> actorCollector)
    {
        _id = id;
        _actorCollector = actorCollector;
        _displayName = id;
        _textColor = "White";
        _interactPosition = RelativePosition.None;
        _interactDirection = Direction.Front;
        _isTouchable = true;
        _isVisible = true;
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

    public Actor Add()
    {
        var actor = new Actor(
            _id,
            _displayName,
            _textColor,
            _interactPosition,
            _interactDirection,
            _isTouchable,
            _isVisible);

        _actorCollector.Add(actor);

        return actor;
    }
}