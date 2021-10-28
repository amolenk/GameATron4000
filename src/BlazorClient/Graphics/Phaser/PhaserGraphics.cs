namespace Amolenk.GameATron4000.Engine.Graphics.Phaser;

public class PhaserGraphics : IGraphics
{
    private IJSRuntime _jsRuntime;
    private IJSInProcessRuntime _jsInProcessRuntime;
    private ILoggerFactory _loggerFactory;

    public int Width { get; }

    public int Height { get; }

    public PhaserGraphics(
        int width,
        int height,
        IJSRuntime jsRuntime,
        ILoggerFactory loggerFactory)
    {
        _jsRuntime = jsRuntime;
        _jsInProcessRuntime = (IJSInProcessRuntime)jsRuntime;
        _loggerFactory = loggerFactory;

        Width = width;
        Height = height;
    }

    public ValueTask AddImageAsync(
        int x,
        int y,
        string key) =>
        _jsRuntime.InvokeVoidAsync(
            PhaserConstants.Functions.AddImage,
            x,
            y,
            PhaserConstants.ImagesKey,
            key);

    public async ValueTask<ISprite> AddSpriteAsync(
        int x,
        int y,
        string key,
        Action<SpriteOptions>? configure)
    {
        var id = Guid.NewGuid().ToString();
        PhaserSpriteInfo spriteInfo;

        var options = new SpriteOptions();
        if (configure != null)
        {
            configure(options);
        }

        spriteInfo = await _jsRuntime.InvokeAsync<PhaserSpriteInfo>(
            PhaserConstants.Functions.AddSprite,
            id,
            x,
            y,
            PhaserConstants.ImagesKey,
            key,
            options);

        return new PhaserSprite(
            spriteInfo,
            x,
            y,
            _jsRuntime,
            _loggerFactory.CreateLogger<PhaserSprite>());
    }

    public ValueTask AddTextAsync(int x, int y, string text) =>
        _jsRuntime.InvokeVoidAsync(
            PhaserConstants.Functions.AddText,
            x,
            y,
            text);

    public void DrawLines(IEnumerable<Line> lines, int lineWidth, int color) =>
        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.DrawLines,
            lines,
            lineWidth,
            color);

    public async ValueTask SetWorldBoundsAsync(int x, int y, int width, int height)
    {
        await _jsRuntime.InvokeVoidAsync(
            PhaserConstants.Functions.SetWorldBounds,
            x,
            y,
            width,
            height);
    }
}
