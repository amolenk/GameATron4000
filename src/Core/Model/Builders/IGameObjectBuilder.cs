namespace Amolenk.GameATron4000.Model.Builders;

public interface IGameObjectBuilder
{
    string DisplayName { get; }
    RelativePosition InteractPosition { get; }
    string InteractStatus { get; }
    bool IsTouchable { get; }
    int ScrollFactor { get; }
    int DepthOffset { get; }
    string Status { get; }
    ActionHandlersBuilder When { get; }

}
