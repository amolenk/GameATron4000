namespace Amolenk.GameATron4000.Mediator;

public interface IMediator
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;

    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    void Subscribe<TMessage>(Func<TMessage, Task> handler) where TMessage : IMessage;
}