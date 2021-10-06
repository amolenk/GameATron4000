namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserSceneCallbacks
{
    private readonly Func<Task> _onPreload;
    private readonly Func<Task> _onCreate;
    private readonly Func<Task> _onUpdate;

    public PhaserSceneCallbacks(
        Func<Task> onPreload,
        Func<Task> onCreate,
        Func<Task> onUpdate)
    {
        _onPreload = onPreload;
        _onCreate = onCreate;
        _onUpdate = onUpdate;
    }

    [JSInvokable]
    public Task PreloadAsync() => _onPreload();

    [JSInvokable]
    public Task CreateAsync() => _onCreate();

    [JSInvokable]
    public Task UpdateAsync() => _onUpdate();
}
