namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEngine(
        this IServiceCollection services,
        Uri baseAddress)
    {
        services
            .AddSingleton<CustomServiceFactory>()
            .AddSingleton<ICustomMediator, CustomMediator>();

        services.AddHttpClient<GameCatalog>(
            client => client.BaseAddress = baseAddress);

        return services;
    }
}
