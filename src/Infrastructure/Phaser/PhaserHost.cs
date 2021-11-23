namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserHost
{
    private readonly GameManifest _manifest;
    private readonly int _width;
    private readonly int _height;
    private readonly IJSInProcessRuntime _jsRuntime;
    private readonly Action<Point> _onDraw;
    private readonly TaskCompletionSource<IGraphics> _startTaskCompletionSource;

    public PhaserHost(
        GameManifest manifest,
        int width,
        int height,
        IJSInProcessRuntime jsRuntime,
        Action<Point> onDraw)
    {
        _manifest = manifest;
        _width = width;
        _height = height;
        _jsRuntime = jsRuntime;
        _onDraw = onDraw;
        _startTaskCompletionSource = new();
    }

    public async Task<IGraphics> StartAsync(string containerElementId)
    {
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.StartPhaser,
            containerElementId,
            _width,
            _height,
            DotNetObjectReference.Create(this)); // TODO Dispose

        return await _startTaskCompletionSource.Task;
    }

    [JSInvokable]
    public void OnPreload()
    {
        foreach (var atlas in _manifest.Spec.Atlasses)
        {
            _jsRuntime.InvokeVoid(
                PhaserConstants.Functions.LoadAtlas,
                atlas.Key,
                _manifest.BasePath + atlas.Value.TextureUrl,
                _manifest.BasePath + atlas.Value.AtlasUrl);
        }

    }

    [JSInvokable]
    public void OnCreate()
    {
        var graphics = new PhaserGraphics(
            _width,
            _height,
            _jsRuntime);

        _startTaskCompletionSource.SetResult(graphics);
    }

    [JSInvokable]
    public void OnUpdate(Point mousePosition) => _onDraw(mousePosition);
}
