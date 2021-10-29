namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserSprite : ISprite
{
    private readonly IJSInProcessRuntime _jsRuntime;
    private Action<Point>? _onPointerDown;

    public string Key { get; private set; }
    public Point Position { get; private set; }
    public Size Size { get; private set; }

    private PhaserSprite(string key, Point position, Size size, IJSInProcessRuntime jsRuntime)
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

    public void OnPointerDown(Action<Point> handler)
    {
        _onPointerDown = handler;

        ((IJSInProcessRuntime)_jsRuntime).InvokeVoid(
            PhaserConstants.Functions.SetSpriteInteraction,
            Key,
            PhaserConstants.Input.Events.PointerDown,
            DotNetObjectReference.Create(this),
            nameof(OnPointerDown));
    }

    public ValueTask SetAnchorAsync(double value)
    {
        throw new NotImplementedException();
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
        Action<Point> onComplete)
    {
        IJSInProcessRuntime js = (IJSInProcessRuntime)_jsRuntime;

        // TODO Rename to PhaserSpriteTween
        return PhaserSpriteTween.MoveSprite(
            this,
            target,
            duration,
            position => Position = position,
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

        var size = jsRuntime.Invoke<Size>(
            PhaserConstants.Functions.AddSprite,
            spriteKey,
            textureKey,
            frameKey,
            position,
            options);

        return new PhaserSprite(spriteKey, position, size, jsRuntime);
    }
}
