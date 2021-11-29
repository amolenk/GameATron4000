namespace Amolenk.GameATron4000.Messages.Events;

public record PlayerActionCompleted(bool OnlyMoved) : IEvent;
