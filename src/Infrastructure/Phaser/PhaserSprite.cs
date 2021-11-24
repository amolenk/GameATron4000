namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public sealed class PhaserSprite : ISprite
{
    private readonly IJSInProcessRuntime _jsRuntime;
    private readonly IEnumerable<IDisposable> _disposables;

    public string Key { get; private set; }
    public Point Position { get; private set; }
    public Size Size { get; private set; }
    public string Frame { get; private set; }

    private PhaserSprite(
        string key,
        Point position,
        Size size,
        string frameKey,
        IEnumerable<IDisposable> disposables,
        IJSInProcessRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _disposables = disposables;

        Key = key;
        Position = position;
        Size = size;
        Frame = frameKey;
    }

    public static ISprite Create(
        string textureKey,
        string frameKey,
        Point position,
        SpriteOptions options,
        IJSInProcessRuntime jsRuntime)
    {
        var spriteKey = Guid.NewGuid().ToString();
        List<IDisposable> disposables = new();

        DotNetObjectReference<PhaserCallback<Point, Task>>? onPointerDownRef = null;
        DotNetObjectReference<PhaserCallback<Point, Task>>? onPointerOutRef = null;
        DotNetObjectReference<PhaserCallback<Point, Task>>? onPointerOverRef = null;

        if (options.OnPointerDown is not null)
        {
            onPointerDownRef = DotNetObjectReference.Create(
                new PhaserCallback<Point, Task>(options.OnPointerDown));

            disposables.Add(onPointerDownRef);
        }

        if (options.OnPointerOut is not null)
        {
            onPointerOutRef = DotNetObjectReference.Create(
                new PhaserCallback<Point, Task>(options.OnPointerOut));

            disposables.Add(onPointerOutRef);
        }

        if (options.OnPointerOver is not null)
        {
            onPointerOverRef = DotNetObjectReference.Create(
                new PhaserCallback<Point, Task>(options.OnPointerOver));

            disposables.Add(onPointerOverRef);
        }

        var size = jsRuntime.Invoke<Size>(
            PhaserConstants.Functions.AddSprite,
            spriteKey,
            textureKey,
            frameKey,
            position,
            options.Origin,
            options.Depth,
            onPointerDownRef,
            onPointerOutRef,
            onPointerOverRef,
            options.ScrollFactor);

        return new PhaserSprite(
            spriteKey,
            position,
            size,
            frameKey,
            disposables,
            jsRuntime);
    }

    public async Task MoveAsync(
        Point target,
        double duration,
        Action onUpdate,
        CancellationToken cancellationToken)
    {
        // MoveAsync uses a Phaser tween to change the x/y position of the
        // sprite. Use a TaskCompletionSource to wait until the tween is
        // done.
        var tcs = new TaskCompletionSource();

        // When the tween has updated the sprite, check if cancellation
        // is requested, and inform the caller of the new position.
        var onUpdateHandler = new PhaserCallback<Point, bool>(
            pointerPosition =>
            {
                Position = pointerPosition;

                onUpdate();

                return !cancellationToken.IsCancellationRequested;
            });

        // When the tween has completed, set the result on the
        // TaskCompletionSource to allow this method to run to completion.
        var onCompleteHandler = new PhaserCallback<Point>(
            pointerPosition =>
            {
                Position = pointerPosition;

                tcs.SetResult();
            });

        // Create DotNetObjectReference instances for the callbacks. These
        // need to be disposed once we're done with them.
        using var onUpdateRef = DotNetObjectReference.Create(onUpdateHandler);
        using var onCompleteRef = DotNetObjectReference.Create(onCompleteHandler);

        // Start the tween.
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.MoveSprite,
            Key,
            target.X,
            target.Y,
            duration,
            onUpdateRef,
            onCompleteRef);

        // Wait for tween to complete.
        await tcs.Task;
    }
    public void SetDepth(double depth) =>
        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            PhaserConstants.Functions.SetSpriteDepth,
            Key,
            depth);

    public void SetFrame(string frame)
    {
        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            PhaserConstants.Functions.SetSpriteFrame,
            Key,
            frame);

        Frame = frame;
    }

    public void AddAnimation(
        string key,
        string atlasKey,
        SpriteAnimationSpec spec) =>
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.AddSpriteAnimation,
            Key,
            key,
            atlasKey,
            spec);

    public void PlayAnimation(string animationKey)
    {
        if (Frame != animationKey)
        {
            _jsRuntime.InvokeVoid(
                PhaserConstants.Functions.PlaySpriteAnimation,
                Key,
                animationKey);

            Frame = animationKey;
        }
    }

    public void StopAnimation()
    {
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.StopSpriteAnimation,
            Key);
    }

    public void Dispose()
    {
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.DestroySprite,
            Key);

        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
