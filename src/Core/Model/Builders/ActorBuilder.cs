namespace Amolenk.GameATron4000.Model.Builders;

public class ActorBuilder : GameObjectBuilder<Actor, ActorBuilder>
{
    private string _textColor;

    internal ActorBuilder(string id, string spriteId, Game game) : base(id, spriteId, game)
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
        _spriteId,
        BuildActionHandlers(),
        _displayName,
        _interactPosition,
        _interactPositionOffsetX,
        _interactPositionOffsetY,
        _interactStatus,
        _isTouchable,
        _scrollFactor,
        _depthOffset,
        _status,
        _textColor);
}
