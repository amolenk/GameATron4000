namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserSprite : ISprite
{
    private readonly IJSInProcessRuntime _jsRuntime;
    private Action<Point>? _onPointerOver;
    private Action<Point>? _onPointerOut;
    private Action<Point>? _onPointerDown;

    public string Key { get; private set; }
    public Point Position { get; private set; }
    public Size Size { get; private set; }

    private PhaserSprite(
        string key,
        Point position,
        Size size,
        IEnumerable<IDisposable> disposables,
        IJSInProcessRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        Key = key;
        Position = position;
        Size = size;
    }

    public void AddAnimation(string key, string framePrefix, int frameStart, int frameEnd, int frameZeroPad, int frameRate, int repeat, int repeatDelay)
    {
        _jsRuntime.InvokeVoid(
            "addSpriteAnimation",
            Key,
            key,
            "images",
            framePrefix,
            frameStart,
            frameEnd,
            frameZeroPad,
            frameRate,
            repeat,
            repeatDelay);
    }

    public void PlayAnimation(string animationKey) =>
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.PlaySpriteAnimation,
            Key,
            animationKey);

    public void StopAnimation() =>
        _jsRuntime.InvokeVoid(
            PhaserConstants.Functions.StopSpriteAnimation,
            Key);

    public void SetFrame(string frameName)
    {
        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            "setSpriteFrame",
            Key,
            frameName);
    }

    public void SetDepth(double depth)
    {
        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            "setSpriteDepth",
            Key,
            depth);
    }

    [JSInvokable]
    public void OnPointerOver(Point mousePosition)
    {
        if (_onPointerOver != null)
        {
            _onPointerOver(mousePosition);
        }
    }

    [JSInvokable]
    public void OnPointerOut(Point mousePosition)
    {
        if (_onPointerOut != null)
        {
            _onPointerOut(mousePosition);
        }
    }

    [JSInvokable]
    public void OnPointerDown(Point mousePosition)
    {
        if (_onPointerDown != null)
        {
            _onPointerDown(mousePosition);
        }
    }

    public ISpriteTween Move(
        Point target,
        int duration,
        Action<Point> onUpdate,
        Action<Point> onComplete)
    {
        IJSInProcessRuntime js = (IJSInProcessRuntime)_jsRuntime;

        // TODO Rename to PhaserSpriteTween
        return PhaserSpriteTween.MoveSprite(
            this,
            target,
            duration,
            position =>
            {
                onUpdate(position);
                Position = position;
            },
            onComplete,
            js);
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

        DotNetObjectReference<PhaserPointerCallback>? onPointerDown = null;
        DotNetObjectReference<PhaserPointerCallback>? onPointerOut = null;
        DotNetObjectReference<PhaserPointerCallback>? onPointerOver = null;

        if (TryCreateDotNetObjectRef(options.OnPointerDown, out onPointerDown))
        {
            dotNetObjectRefs.Add(onPointerDown!);
        }

        if (TryCreateDotNetObjectRef(options.OnPointerOut, out onPointerOut))
        {
            dotNetObjectRefs.Add(onPointerOut!);
        }

        if (TryCreateDotNetObjectRef(options.OnPointerOver, out onPointerOver))
        {
            dotNetObjectRefs.Add(onPointerOver!);
        }

        var size = jsRuntime.Invoke<Size>(
            PhaserConstants.Functions.AddSprite,
            spriteKey,
            textureKey,
            frameKey,
            position,
            options.Origin,
            options.Depth,
            onPointerDown,
            onPointerOut,
            onPointerOver);


        return new PhaserSprite(
            spriteKey,
            position,
            size,
            dotNetObjectRefs,
            jsRuntime);
    }

    private static bool TryCreateDotNetObjectRef(
        Func<Point, Task>? callback,
        out DotNetObjectReference<PhaserPointerCallback>? dotNetObjectRef)
    {
        if (callback is not null)
        {
            dotNetObjectRef = DotNetObjectReference.Create(
                new PhaserPointerCallback(callback));
            return true;
        }

        dotNetObjectRef = null;
        return false;
    }
}
