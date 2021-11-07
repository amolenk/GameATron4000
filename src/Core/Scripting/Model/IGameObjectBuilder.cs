namespace Amolenk.GameATron4000.Scripting.Model;

// TODO Used?
public interface IGameObjectBuilder
{
    string Id { get; }
    string DisplayName { get; }
    string State { get; }
    bool IsTouchable { get; }
    int ScrollFactor { get; }
    RelativePosition InteractPosition { get; }
    string InteractState { get; }
    ActionHandlers ActionHandlers { get; }
    Game Game { get; }
}
