namespace Amolenk.GameATron4000.Infrastructure.Mediator;

// TODO Allow unsubscribe/clear

/// <summary>
/// Keeps track of Mediatr handlers.
/// Mediatr provides a Mediatr.Extensions.Microsoft.DependencyInjection package
/// to scan assemblies and add handlers, preprocessors and postprocessors to the
/// .NET IoC container. All handlers are registered as transient, which results
/// in multiple instances of message-handling Blazor components being created.
/// This class solves the problem by enabling handler instances to subscribe
/// (and unsubscribe) dynamically to/from Mediatr messages.
/// </summary>
public class CustomServiceFactory
{
    private record ServiceRegistration(Type Type, object Instance);

    private readonly List<ServiceRegistration> _registrations;

    public CustomServiceFactory()
    {
        _registrations = new List<ServiceRegistration>();
    }

    public void Subscribe<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest
    {
        _registrations.Add(
            new ServiceRegistration(
                typeof(IRequestHandler<TRequest, Unit>),
                handler));
    }

    public void Subscribe<TNotification>(
        INotificationHandler<TNotification> handler)
        where TNotification : INotification
    {
        _registrations.Add(
            new ServiceRegistration(
                typeof(INotificationHandler<TNotification>),
                handler));
    }

    public object Resolve(Type serviceType) =>
        IsEnumerable(serviceType)
            ? ResolveInstances(serviceType)
            : ResolveInstance(serviceType);

    private object ResolveInstances(Type serviceType)
    {
        var typeArgument = serviceType.GetGenericArguments().Single();

        var result = CreateEmptyList(typeArgument);

        foreach (var match in _registrations
            .Where(registration => registration.Type == typeArgument))
        {
            result.Add(match.Instance);
        }

        return result;
    }

    private object ResolveInstance(Type serviceType) =>
        _registrations
            .Where(registration => registration.Type == serviceType)
            .Select(registration => registration.Instance)
            .FirstOrDefault()!;

    private bool IsEnumerable(Type type) =>
        type.IsGenericType
        && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

    private IList CreateEmptyList(Type itemType) =>
        (IList)Activator.CreateInstance(
            typeof(List<>).MakeGenericType(itemType))!;
}
