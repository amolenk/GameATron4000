namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectFrameChanged(
    GameObject GameObject,
    string Frame,
    IEnumerable<GameObject> ObjectsToHide,
    IEnumerable<GameObject> ObjectsToShow)
    : IEvent;
