namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaveGameRepository(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddBlazoredLocalStorage(config =>
            config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        );

        serviceCollection.AddScoped<ISaveGameRepository, LocalStorageSaveGameRepository>();

        return serviceCollection;
    }
}