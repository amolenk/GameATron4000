namespace Amolenk.GameATron4000.Engine;

public class GameCatalog
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;

    public GameCatalog(HttpClient client, ILogger<GameCatalog> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<GameManifest>> LoadCatalogAsync()
    {
        List<GameManifest> result = new();

        var diskNumber = 1;
        var diskExists = true;

        while (diskExists)
        {
            var manifest = await TryLoadGameManifest(diskNumber++);
            if (manifest != null)
            {
                result.Add(manifest);
            }
            else
            {
                diskExists = false;
            }
        }

        _logger.LogDebug("Found {Count} game(s)", result.Count);

        return result.AsReadOnly();
    }

    private async Task<GameManifest?> TryLoadGameManifest(int diskNumber)
    {
        var basePath = $"games/disk{diskNumber:000}/";

        var response = await _client.GetAsync($"{basePath}/game.yaml");

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var manifest = deserializer.Deserialize<GameManifest>(content);
                manifest.BasePath = basePath;

                return manifest;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading game {DiskNumber}",
                    diskNumber);
            }
        }

        return null;
    }
}
