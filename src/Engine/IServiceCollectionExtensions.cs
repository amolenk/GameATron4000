namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEngine(
        this IServiceCollection services,
        Uri baseAddress)
    {
        services
            .AddScoped<IDynamicMediator>(sp =>
                new DynamicMediator(new MediatorServiceFactory()));
            //.AddScoped<MediatorServiceFactory>()
            //.AddScoped<ServiceFactory>(
            //    sp => sp.GetRequiredService<MediatorServiceFactory>().Resolve)
            //.AddScoped<IMediator, Mediator>();

        services.AddHttpClient<GameCatalog>(
            client => client.BaseAddress = baseAddress);

        return services;
    }
}
