namespace Amolenk.GameATron4000.Model.Builders;

public class ActorBuilder : GameObjectBuilder<Actor, ActorBuilder>
{
    private string _textColor;

    internal ActorBuilder(string id, Game game) : base(id, game)
    {
        _status = WellKnownStatus.FaceCamera;
        _textColor = "white";
    }

    public ActorBuilder WithTextColor(string textColor)
    {
        _textColor = textColor;
        return this;
    }

    public override Actor Build() => new Actor(
        _game,
        _id,
        BuildActionHandlers(),
        _displayName,
        _interactPosition,
        _interactStatus,
        _isTouchable,
        _scrollFactor,
        _status,
        _textColor);
}
