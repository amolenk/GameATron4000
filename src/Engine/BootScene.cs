namespace Amolenk.GameATron4000.Engine;

public class BootScene : Scene
{
    private readonly GameManifest _manifest;
    private readonly IMediator _mediator;

    public BootScene(GameManifest manifest, IMediator mediator) : base("_boot")
    {
        _manifest = manifest;
        _mediator = mediator;
    }

    public override Task PreloadAsync(IAssetLoader loader)
    {
        List<Task> loadTasks = new();

        foreach (var imageSpec in _manifest.Spec.Images)
        {
            loadTasks.Add(
                loader.LoadImageAsync(imageSpec.Key, imageSpec.ImageUrl)
                .AsTask());
        }

        foreach (var atlasSpec in _manifest.Spec.Atlases)
        {
            loadTasks.Add(
                loader.LoadAtlasAsync(
                    atlasSpec.Key,
                    atlasSpec.TextureUrl,
                    atlasSpec.AtlasUrl)
                .AsTask());
        }

        return Task.WhenAll(loadTasks);
    }

    public override async Task CreateAsync(IGraphics graphics)
    {
        await graphics.AddTextAsync(100, 100, "Publishing RoomViewReadyEvent notification...");

        await _mediator.Publish(new RoomViewReadyEvent());
    }
}
