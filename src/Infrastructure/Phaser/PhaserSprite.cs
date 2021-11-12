namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public sealed class PhaserSprite : ISprite
{
    private readonly IJSInProcessRuntime _jsRuntime;
    private readonly IEnumerable<IDisposable> _disposables;

    public string Key { get; private set; }
    public Point Position { get; private set; }
    public Size Size { get; private set; }
    public string? PlayingAnimation { get; private set; }

    private PhaserSprite(
        string key,
        Point position,
        Size size,
        IEnumerable<IDisposable> disposables,
        IJSInProcessRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _disposables = disposables;

        Key = key;
        Position = position;
        Size = size;
    }

    public static ISprite Create(
        string textureKey,
        string frameKey,
        Point position,
        SpriteOptions options,
        IJSInProcessRuntime jsRuntime)
    {
        var spriteKey = Guid.NewGuid().ToString();
        var dotNetObjectRefs = new List<IDisposable>();

        DotNetObjectReference<PhaserFuncCallback<Point, Task>>? onPointerDownRef = null;
        DotNetObjectReference<PhaserFuncCallback<Point, Task>>? onPointerOutRef = null;
        DotNetObjectReference<PhaserFuncCallback<Point, Task>>? onPointerOverRef = null;

        if (TryCreateDotNetObjectRef(options.OnPointerDown, out onPointerDownRef))
        {
            dotNetObjectRefs.Add(onPointerDownRef!);
        }

        if (TryCreateDotNetObjectRef(options.OnPointerOut, out onPointerOutRef))
        {
            dotNetObjectRefs.Add(onPointerOutRef!);
        }

        if (TryCreateDotNetObjectRef(options.OnPointerOver, out onPointerOverRef))
        {
            dotNetObjectRefs.Add(onPointerOverRef!);
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
            dotNetObjectRefs,
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
        var onUpdateHandler = new PhaserFuncCallback<Point, bool>(
            pointerPosition =>
            {
                Position = pointerPosition;

                onUpdate();

                return !cancellationToken.IsCancellationRequested;
            });

        // When the tween has completed, set the result on the
        // TaskCompletionSource to allow this method to run to completion.
        var onCompleteHandler = new PhaserActionCallback<Point>(
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

    public void SetFrame(string frameName) =>
        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            PhaserConstants.Functions.SetSpriteFrame,
            Key,
            frameName);

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
        if (PlayingAnimation != animationKey)
        {
            _jsRuntime.InvokeVoid(
                PhaserConstants.Functions.PlaySpriteAnimation,
                Key,
                animationKey);

            PlayingAnimation = animationKey;
        }
    }

    public void StopAnimation()
    {
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.StopSpriteAnimation,
            Key);

        PlayingAnimation = null;
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

    private static bool TryCreateDotNetObjectRef(
        Func<Point, Task>? callback,
        out DotNetObjectReference<PhaserFuncCallback<Point, Task>>? dotNetObjectRef)
    {
        if (callback is not null)
        {
            dotNetObjectRef = DotNetObjectReference.Create(
                new PhaserFuncCallback<Point, Task>(callback));
            return true;
        }

        dotNetObjectRef = null;
        return false;
    }
}
