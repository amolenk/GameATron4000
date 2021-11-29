namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaveGameRepository(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIndexedDB(dbStore =>
        {
            dbStore.DbName = "GameATron";
            dbStore.Version = 1;

            dbStore.Stores.Add(new StoreSchema
            {
                Name = IndexedDBSaveGameRepository.StoreName,
                PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id" }
            });
        });

        serviceCollection.AddScoped<ISaveGameRepository, IndexedDBSaveGameRepository>();

        return serviceCollection;
    }
}