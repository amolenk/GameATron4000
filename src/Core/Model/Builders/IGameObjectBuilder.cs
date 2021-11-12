namespace Amolenk.GameATron4000.Model.Builders;

public interface IGameObjectBuilder
{
    string Id { get; }
    string DisplayName { get; }
    bool IsTouchable { get; }
    bool UseWith { get; }
    string FrameName { get; }
    string InteractFrameName { get; }
    RelativePosition InteractPosition { get; }
    GameObjectCondition Condition { get; }
    int ScrollFactor { get; }
    IGameObjectHandlersBuilder HandlersBuilder { get; }
    Game Game { get; }
}
