namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectStatusChanged(
    IGameObject GameObject,
    string Status,
    IEnumerable<IGameObject> ObjectsToHide,
    IEnumerable<IGameObject> ObjectsToShow)
    : IEvent;
