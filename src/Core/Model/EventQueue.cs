namespace Amolenk.GameATron4000.Model;

public class EventQueue
{
    private readonly Queue<IEvent> _events;

    public Func<IEvent, bool> Filter { get; private set; }
    public int Count => _events.Count;

    public EventQueue()
    {
        _events = new();

        AllowAll();
    }

    public void Enqueue(IEvent @event)
    {
        if (Filter(@event))
        {
            _events.Enqueue(@event);
        }
    }

    public void SetFilter(Func<IEvent, bool> filter) => Filter = filter;

    public void IgnoreAll() => SetFilter(_ => false);

    public void AllowAll() => SetFilter(_ => true);

    public async Task FlushAsync(IMediator mediator)
    {
        foreach (var @event in _events)
        {
            await mediator.PublishAsync(@event);
        }

        _events.Clear();
    }
}
