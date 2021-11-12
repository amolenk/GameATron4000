namespace Amolenk.GameATron4000.Messages.Events;

public record GameObjectFrameChanged(
    GameObject GameObject,
    string FrameName,
    IEnumerable<GameObject> ObjectsToHide,
    IEnumerable<GameObject> ObjectsToShow)
    : IEvent;
