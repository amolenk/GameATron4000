namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserFuncCallback<TArg, TResult>
{
    private readonly Func<TArg, TResult> _handler;

    public PhaserFuncCallback(Func<TArg, TResult> handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public TResult Invoke(TArg arg) => _handler(arg);
}