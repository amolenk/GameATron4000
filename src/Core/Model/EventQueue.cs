namespace Amolenk.GameATron4000.Model;

public class EventQueue
{
    private readonly Queue<IEvent> _events;
    private readonly IMediator _mediator;

    public Func<IEvent, bool> Filter { get; private set; }
    public int Count => _events.Count;

    public EventQueue(IMediator mediator)
    {
        _events = new();
        _mediator = mediator;

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

    public async Task FlushAsync()
    {
        foreach (var @event in _events)
        {
            await _mediator.PublishAsync(@event);
        }

        _events.Clear();
    }
}
