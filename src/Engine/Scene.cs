namespace Amolenk.GameATron4000.Engine;

public class BootScene : PhaserScene
{
    public BootScene() : base("_boot")
    {
    }

    protected override Task PreloadAsync()
        => Task.WhenAll(CreateLoadAssetTasks());

    protected override async Task CreateAsync()
    {

        await AddImageAsync(0, 0, "assets", "rooms/park");

        await AddImageAsync(200, 200, "pretzel");

        await AddTextAsync(100, 100, $"Loaded {Manifest.Metadata.Name}!");
    }

    private IEnumerable<Task> CreateLoadAssetTasks()
    {
        foreach (var imageSpec in Manifest.Spec.Images)
        {
            yield return LoadImageAsync(imageSpec.Key, imageSpec.ImageUrl)
                .AsTask();
        }

        foreach (var atlasSpec in Manifest.Spec.Atlases)
        {
            yield return LoadAtlasAsync(
                atlasSpec.Key,
                atlasSpec.TextureUrl,
                atlasSpec.AtlasUrl)
                .AsTask();
        }
    }
}
