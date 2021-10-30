namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserPointerCallback
{
    private readonly Func<Point, Task> _handler;

    public PhaserPointerCallback(Func<Point, Task> handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public Task InvokeAsync(Point pointerPosition) => _handler(pointerPosition);
}