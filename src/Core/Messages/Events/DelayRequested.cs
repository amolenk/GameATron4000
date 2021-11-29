namespace Amolenk.GameATron4000.Messages.Events;

public record DelayRequested(TimeSpan Delay) : IEvent;
