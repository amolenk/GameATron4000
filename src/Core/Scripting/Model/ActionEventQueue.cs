namespace Amolenk.GameATron4000.Scripting.Model;

public class ActionEventQueue
{
    private readonly Queue<IEvent> _events;
    private readonly IMediator _mediator;

    public bool IgnoreNewEvents { get; set; }

    public ActionEventQueue(IMediator mediator)
    {
        _events = new();
        _mediator = mediator;

        IgnoreNewEvents = false;
    }

    public void Enqueue(IEvent @event)
    {
        if (!IgnoreNewEvents)
        {
            _events.Enqueue(@event);
        }
    }

    public async Task FlushAsync()
    {
        foreach (var @event in _events)
        {
            await _mediator.PublishAsync(@event);
        }

        _events.Clear();
    }
}
