namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserSpriteTween : ISpriteTween
{
    private readonly string _id;
    private readonly IJSInProcessRuntime _jsRuntime;
    private readonly Action<Point> _onUpdate;
    private readonly Action<Point> _onComplete;

    public PhaserSpriteTween(string id, Action<Point> onUpdate, Action<Point> onComplete, IJSInProcessRuntime jsRuntime)
    {
        _id = id;
        _onUpdate = onUpdate;
        _onComplete = onComplete;
        _jsRuntime = jsRuntime;
    }

    public void Stop()
    {
        _jsRuntime.InvokeVoid("stopTween", _id);
    }

    [JSInvokable]
    public void OnUpdate(Point position)
    {
        _onUpdate(position);
    }

    [JSInvokable]
    public void OnComplete(Point position) => _onComplete(position);

    public static ISpriteTween MoveSprite(
        ISprite sprite,
        Point target,
        int duration,
        Action<Point> onUpdate,
        Action<Point> onComplete,
        IJSInProcessRuntime jsRuntime)
    {
        var id = Guid.NewGuid().ToString();
        var tween = new PhaserSpriteTween(id, onUpdate, onComplete, jsRuntime);

        jsRuntime.InvokeVoid(
            PhaserConstants.Functions.AddTween,
            id,
            sprite.Id,
            target.X,
            target.Y,
            duration,
            DotNetObjectReference.Create(tween));

        return tween;
    }
}
