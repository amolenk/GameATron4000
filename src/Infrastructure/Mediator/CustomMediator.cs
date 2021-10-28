namespace Amolenk.GameATron4000.Infrastructure.Mediator;

public class CustomMediator : MediatR.Mediator, ICustomMediator
{
    private readonly CustomServiceFactory _serviceFactory;

    public CustomMediator(CustomServiceFactory serviceFactory)
        : base(serviceFactory.Resolve)
    {
        _serviceFactory = serviceFactory;
    }

    public void Subscribe<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest =>
        _serviceFactory.Subscribe(handler);

    public void Subscribe<TNotification>(
        INotificationHandler<TNotification> handler)
        where TNotification : INotification =>
        _serviceFactory.Subscribe(handler);
}
