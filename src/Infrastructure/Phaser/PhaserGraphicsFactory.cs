namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserGraphicsFactory
{
    private readonly IJSInProcessRuntime _jsInProcessRuntime;

    public PhaserGraphicsFactory(IJSInProcessRuntime jsInProcessRuntime)
    {
        _jsInProcessRuntime = jsInProcessRuntime;
    }

    public async Task<IGraphics> CreateAsync(
        GameManifest manifest,
        int width,
        int height,
        Action<Point> onDraw,
        string containerElementId)
    {
        TaskCompletionSource<IGraphics> tcs = new();
        List<IDisposable> disposables = new();

        // When the Phaser scene preloads, load the sprite atlasses from the
        // game manifest.
        var onPreloadHandler = new PhaserCallback(
            () =>
            {
                foreach (var atlas in manifest.Spec.Atlasses)
                {
                    _jsInProcessRuntime.InvokeVoid(
                        PhaserConstants.Functions.LoadAtlas,
                        atlas.Key,
                        manifest.BasePath + atlas.Value.TextureUrl,
                        manifest.BasePath + atlas.Value.AtlasUrl);
                }
            });

        // When the Phaser scene is created, complete the task by setting
        // a new PhaserGraphics as the task result. 
        var onCreateHandler = new PhaserCallback(
            () =>
            {
                var graphics = new PhaserGraphics(
                    width,
                    height,
                    disposables,
                    _jsInProcessRuntime);

                tcs.SetResult(graphics);
            });
        
        var onUpdateHandler = new PhaserCallback<Point>(onDraw);

        // Create DotNetObjectReference instances for the callbacks. These
        // need to be disposed once we're done with them.
        using var onPreloadRef = DotNetObjectReference.Create(onPreloadHandler);
        using var onCreateRef = DotNetObjectReference.Create(onCreateHandler);

        // We cannot dispose the onUpdate callback yet, it will be used until
        // the PhaserGraphics object is disposed. 
        var onUpdateRef = DotNetObjectReference.Create(onUpdateHandler);
        disposables.Add(onUpdateRef);

        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.StartPhaser,
            containerElementId,
            width,
            height,
            onPreloadRef,
            onCreateRef,
            onUpdateRef); 

        return await tcs.Task;
    }
}
