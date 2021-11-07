namespace Amolenk.GameATron4000.Model.Builders;

public interface IGameObjectBuilder
{
    string Id { get; }
    string DisplayName { get; }
    string State { get; }
    bool IsTouchable { get; }
    int ScrollFactor { get; }
    RelativePosition InteractPosition { get; }
    string InteractState { get; }
    IGameObjectHandlersBuilder When { get; }
    Game Game { get; }
}
