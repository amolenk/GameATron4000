namespace Amolenk.GameATron4000.Messages.Events;

public record LineSpoken(Actor Actor, string Line) : IEvent;
