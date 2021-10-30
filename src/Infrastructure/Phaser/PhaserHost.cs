namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserHost
{
    private readonly GameManifest _manifest;
    private readonly int _width;
    private readonly int _height;
    private readonly IJSInProcessRuntime _jsRuntime;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Action<Point> _onDraw;
    private readonly TaskCompletionSource<IGraphics> _startTaskCompletionSource;

    public PhaserHost(
        GameManifest manifest,
        int width,
        int height,
        IJSInProcessRuntime jsRuntime,
        ILoggerFactory loggerFactory,
        Action<Point> onDraw)
    {
        _manifest = manifest;
        _width = width;
        _height = height;
        _jsRuntime = jsRuntime;
        _loggerFactory = loggerFactory;
        _onDraw = onDraw;
        _startTaskCompletionSource = new TaskCompletionSource<IGraphics>();
    }

    public async Task<IGraphics> StartAsync(string containerElementId)
    {
        await _jsRuntime.InvokeVoidAsync(
            PhaserConstants.Functions.StartPhaser,
            containerElementId,
            _width,
            _height,
            DotNetObjectReference.Create(this));

        return await _startTaskCompletionSource.Task;
    }

    [JSInvokable]
    public void OnPreload()
    {
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.LoadAtlas,
            "images",
            _manifest.BasePath + _manifest.Spec.Images.TextureUrl,
            _manifest.BasePath + _manifest.Spec.Images.AtlasUrl);
    }

    [JSInvokable]
    public void OnCreate()
    {
        var graphics = new PhaserGraphics(
            _width,
            _height,
            _jsRuntime,
            _loggerFactory);

        _startTaskCompletionSource.SetResult(graphics);
    }

    [JSInvokable]
    public void OnUpdate(Point mousePosition) => _onDraw(mousePosition);
}
