namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserCallback
{
    private readonly Action _handler;

    public PhaserCallback(Action handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public void Invoke() => _handler();
}

public class PhaserCallback<TArg>
{
    private readonly Action<TArg> _handler;

    public PhaserCallback(Action<TArg> handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public void Invoke(TArg arg) => _handler(arg);
}

public class PhaserCallback<TArg, TResult>
{
    private readonly Func<TArg, TResult> _handler;

    public PhaserCallback(Func<TArg, TResult> handler)
    {
        _handler = handler;
    }

    [JSInvokable]
    public TResult Invoke(TArg arg) => _handler(arg);
}