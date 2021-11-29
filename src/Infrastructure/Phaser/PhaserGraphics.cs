namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserGraphics : IGraphics
{
    private readonly IEnumerable<IDisposable> _disposables;
    private readonly IJSInProcessRuntime _jsInProcessRuntime;

    public int Width { get; }
    public int Height { get; }

    public PhaserGraphics(
        int width,
        int height,
        IEnumerable<IDisposable> disposables,
        IJSInProcessRuntime jsInProcessRuntime)
    {
        _disposables = disposables;
        _jsInProcessRuntime = jsInProcessRuntime;

        Width = width;
        Height = height;
    }

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

    public void CameraFollow(ISprite sprite) =>
        _jsInProcessRuntime.InvokeVoid(
            "cameraFollow",
            sprite.Key);

    public async Task StartCameraFollowAsync(ISprite sprite)
    {
        var tcs = new TaskCompletionSource();

        var onCompleteHandler = new PhaserCallback(tcs.SetResult);

        // Create DotNetObjectReference instances for the callbacks. These
        // need to be disposed once we're done with them.
        using var onCompleteRef = DotNetObjectReference.Create(onCompleteHandler);

        // Start the tween.
        _jsInProcessRuntime.InvokeVoid(
            "startCameraFollow",
            sprite.Key,
            onCompleteRef);

        await tcs.Task;
    }

    public void SetCameraBounds(Size size) =>
        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.SetCameraBounds,
            size);

    public IDisposable Pause()
    {
        _jsInProcessRuntime.InvokeVoid(
            PhaserConstants.Functions.Pause);

        return new ScenePaused(_jsInProcessRuntime);
    }

    public void Dispose() =>
        _jsInProcessRuntime.InvokeVoid(PhaserConstants.Functions.DestroyPhaser);
}
