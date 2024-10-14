using System.Net;

namespace Amolenk.GameATron4000.Model;

public class EventQueue
{
    private Queue<IEvent> _events;

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

    public async Task ResolveApiCallEventsAsync(ExternalApiClient externalApiClient)
    {
        await Task.Yield();
        // var currentEvents = _events;
        // _events = new Queue<IEvent>();
        //
        // while (currentEvents.Count > 0)
        // {
        //     var currentEvent = currentEvents.Dequeue();
        //     if (currentEvent is ApiCallRequested apiCall)
        //     {
        //         var result = await externalApiClient.GetAsync(apiCall.RequestUri, apiCall.Claims);
        //         
        //         if (result.StatusCode == HttpStatusCode.OK)
        //         {
        //             apiCall.OnSuccess(result.Content);
        //         }
        //         else
        //         {
        //             apiCall.OnFailure(result.Content);
        //         }
        //     }
        //     else
        //     {
        //         _events.Enqueue(currentEvent);
        //     }
        // }
    }

    public async Task FlushAsync(IMediator mediator, ExternalApiClient externalApiClient)
    {
        var currentEvents = _events.ToList();
        _events.Clear();
        
        foreach (var @event in currentEvents)
        {
            if (@event is ApiCallRequested apiCall)
            {
                var result = await externalApiClient.GetAsync(apiCall.RequestUri, apiCall.Claims);
                
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    apiCall.OnSuccess(result.Content);
                }
                else
                {
                    apiCall.OnFailure(result.Content);
                }

                await FlushAsync(mediator, externalApiClient);
            }
            else
            {
                await mediator.PublishAsync(@event);
            }
        }
        //
        // _events.Clear();
    }
    
    // private static async Task PublishEventsAsync(IEnumerable<IEvent> events, IMediator mediator, ExternalApiClient externalApiClient)
    // {
    //     foreach (var @event in events)
    //     {
    //         if (@event is ApiCallRequested apiCall)
    //         {
    //             var result = await externalApiClient.GetAsync(apiCall.RequestUri, apiCall.Claims);
    //             
    //             if (result.StatusCode == HttpStatusCode.OK)
    //             {
    //                 await PublishEventsAsync(apiCall.OnSuccess)
    //                 apiCall.OnSuccess(result.Content);
    //             }
    //             else
    //             {
    //                 apiCall.OnFailure(result.Content);
    //             }
    //         }
    //         
    //         await mediator.PublishAsync(@event);
    //     }
    //
    //     _events.Clear();
    // }
}
