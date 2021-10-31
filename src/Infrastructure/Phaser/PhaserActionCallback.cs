namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserActionCallback<TArg>
{
    private readonly Action<TArg> _handler;

    public PhaserActionCallback(Action<TArg> handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public void Invoke(TArg arg) => _handler(arg);
}