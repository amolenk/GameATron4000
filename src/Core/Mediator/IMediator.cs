namespace Amolenk.GameATron4000.Mediator;

public interface IMediator
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;

    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    void Subscribe<T>(Func<T, Task> handler);
}