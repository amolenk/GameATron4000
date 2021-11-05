namespace Amolenk.GameATron4000.Infrastructure.Phaser;

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
            "images",
            key);

    public ISprite AddSprite(
        string textureKey,
        string frameKey,
        Point position,
        Action<SpriteOptions>? configure)
    {
        SpriteOptions options = new();
        if (configure is not null)
        {
            configure(options);
        }

        return PhaserSprite.Create(
            textureKey,
            frameKey,
            position,
            options,
            _jsInProcessRuntime);
    }

    public IText AddText(
        string text,
        Point position,
        Action<TextOptions>? configure)
    {
        TextOptions options = new();
        if (configure is not null)
        {
            configure(options);
        }

        return PhaserText.Create(
            text,
            position,
            options,
            _jsInProcessRuntime);
    }

    public void DrawLines(IEnumerable<Line> lines, int lineWidth, int color) =>
        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.DrawLines,
            lines,
            lineWidth,
            color);

    public Point GetCameraPosition() =>
        _jsInProcessRuntime.Invoke<Point>(
            "getCameraPosition"); // TODO

    public void StartCameraFollow(ISprite sprite) =>
        _jsInProcessRuntime.InvokeVoid(
            "startCameraFollow",
            sprite.Key);

    public void SetWorldBounds(Size size) =>
        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.SetWorldBounds,
            size);
}
