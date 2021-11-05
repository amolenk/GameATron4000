namespace Amolenk.GameATron4000.Messages.Events;

public record PlayerActionCompleted(IAction Action) : IEvent;
