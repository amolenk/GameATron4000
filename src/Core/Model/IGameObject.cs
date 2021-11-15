namespace Amolenk.GameATron4000.Model;

// TODO Remove again!
public interface IGameObject
{
    string Id { get; }
    string DisplayName { get; }
    RelativePosition InteractPosition { get; }
    string InteractStatus { get; }
    bool IsTouchable { get; }
    int ScrollFactor { get; }
    bool IsVisible { get; }

    Point Position { get; }
    string Status { get; }

    ActionHandlers ActionHandlers { get; }

    void UpdatePosition(Point position);
}
