namespace Amolenk.GameATron4000.Infrastructure.Mediator;

public class DefaultMediator : GameATron4000.Mediator.IMediator
{
    private readonly Dictionary<Type, List<Func<object, Task>>> _handlersByType;

    public DefaultMediator()
    {
        _handlersByType = new();
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var eventType = @event.GetType();

        if (_handlersByType.TryGetValue(
            @event.GetType(),
            out List<Func<object, Task>> handlers))
        {
            var tasks = handlers
                .Select(handler => handler.Invoke(@event));

            await Task.WhenAll(tasks);
        }
    }

    public async Task SendAsync<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        if (_handlersByType.TryGetValue(
            typeof(TCommand),
            out List<Func<object, Task>> handlers))
        {
            if (handlers.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Multiple handlers found for command '{typeof(TCommand)}' when maximum 1 was expected.");
            }

            await handlers[0].Invoke(command);
        }
    }

    public void Subscribe<TMessage>(Func<TMessage, Task> handler)
        where TMessage : IMessage
    {
        List<Func<object, Task>> handlers;

        if (!_handlersByType.TryGetValue(typeof(TMessage), out handlers))
        {
            handlers = new();
            _handlersByType.Add(typeof(TMessage), handlers);
        }

        handlers.Add(message => handler((TMessage)message));
    }
}