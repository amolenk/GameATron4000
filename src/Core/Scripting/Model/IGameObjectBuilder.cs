namespace Amolenk.GameATron4000.Scripting.Model;

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
    ICollector<IEvent> Events { get; }

    // TODO Remove
    // GameObject Named(string displayName);

    // GameObject InState(string state);

    // GameObject Untouchable();

    // GameObject WithScrollFactor(int scrollFactor);

    // GameObject InteractFromPosition(RelativePosition position);

    // GameObject InteractInState(string state);

    // GameObject When(Action<ActionHandlers> configure);
}
