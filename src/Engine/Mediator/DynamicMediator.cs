namespace Amolenk.GameATron4000.Engine.Mediator;

public class DynamicMediator : MediatR.Mediator, IDynamicMediator
{
    private readonly MediatorServiceFactory _serviceFactory;

    public DynamicMediator(MediatorServiceFactory serviceFactory)
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
