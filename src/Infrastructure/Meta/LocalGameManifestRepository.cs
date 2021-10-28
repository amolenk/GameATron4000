namespace Amolenk.GameATron4000.Infrastructure.Meta;

public class LocalGameManifestRepository : IGameManifestRepository
{
    private readonly HttpClient _client;
    private readonly ILogger _logger;

    public LocalGameManifestRepository(HttpClient client, ILogger<LocalGameManifestRepository> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<GameManifest>> LoadGameManifestsAsync()
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

    public async Task<GameManifest?> TryLoadGameManifest(int diskNumber)
    {
        var basePath = $"games/disk{diskNumber:000}/";

        // Add current time to avoid caching.
        var response = await _client.GetAsync(
            $"{basePath}/game.yaml?time={DateTime.Now}");

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
