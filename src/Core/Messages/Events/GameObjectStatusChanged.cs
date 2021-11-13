namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectStatusChanged(
    GameObject GameObject,
    string Status,
    IEnumerable<GameObject> ObjectsToHide,
    IEnumerable<GameObject> ObjectsToShow)
    : IEvent;
