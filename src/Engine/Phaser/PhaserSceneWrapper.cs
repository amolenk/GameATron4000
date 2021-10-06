namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserSceneWrapper<TScene> where TScene : Scene
{
    private readonly Scene _scene;
    private readonly PhaserGraphics _graphics;
    private readonly PhaserLoader _loader;

    public PhaserSceneWrapper(TScene scene, string gameBasePath, IJSRuntime js, ILoggerFactory loggerFactory)
    {
        _scene = scene;
        _graphics = new PhaserGraphics(scene.Id, js, loggerFactory);
        _loader = new PhaserLoader(scene.Id, gameBasePath, js);
    }

    [JSInvokable]
    public Task PreloadAsync() => _scene.PreloadAsync(_loader);

    [JSInvokable]
    public Task CreateAsync() => _scene.CreateAsync(_graphics);

    [JSInvokable]
    public Task UpdateAsync() => _scene.UpdateAsync(_graphics);
}

public class PhaserScene
{
    private readonly PhaserGraphics _graphics;
    private readonly PhaserLoader _loader;
    private readonly IMediator _mediator;

    public PhaserScene(string gameBasePath, IJSRuntime js, ILoggerFactory loggerFactory)
    {
        _graphics = new PhaserGraphics("room", js, loggerFactory);
        _loader = new PhaserLoader("room", gameBasePath, js);
    }

    [JSInvokable]
    public Task PreloadAsync() => _mediator.Send(new LoadRoomAssetsCommand(_loader));

    [JSInvokable]
    public Task CreateAsync() => Task.CompletedTask;

    [JSInvokable]
    public Task UpdateAsync() => Task.CompletedTask;
}