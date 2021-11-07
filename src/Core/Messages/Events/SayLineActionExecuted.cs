namespace Amolenk.GameATron4000.Messages.Events;

public record SayLineActionExecuted(Actor Actor, string Line) : IEvent;
