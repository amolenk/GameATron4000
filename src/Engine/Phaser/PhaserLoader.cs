namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserScene<TScene> where TScene : Scene
{
    private readonly Scene _scene;

    public PhaserScene(TScene scene)
    {
        _scene = scene;
    }

    [JSInvokable]
    public Task PreloadAsync() => _scene.PreloadAsync();

    [JSInvokable]
    public Task CreateAsync() => _scene.CreateAsync();

    [JSInvokable]
    public Task UpdateAsync() => _scene.UpdateAsync();
}
