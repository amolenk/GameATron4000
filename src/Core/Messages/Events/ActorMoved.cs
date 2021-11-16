namespace Amolenk.GameATron4000.Messages.Events;

public record ActorMoved(
    Actor Actor,
    Point Position,
    string EndInStatus)
    : IEvent;
