//namespace Amolenk.GameATron4000.Infrastructure.Phaser;

//public class PhaserScene
//{
//    private readonly IScene _scene;
//    private readonly PhaserLoader _loader;
//    private readonly PhaserGraphics _graphics;

//    public PhaserScene(IScene scene, IJSRuntime js, ILoggerFactory loggerFactory)
//    {
//        _scene = scene;
//        _loader = new PhaserLoader(_scene.Id, js, loggerFactory);
//        _graphics = new PhaserGraphics(scene.Id, js, loggerFactory);
//    }

//    public string Id => _scene.Id;

//    [JSInvokable]
//    public Task PreloadAsync() => _scene.PreloadAsync(_graphics, _loader);

//    [JSInvokable]
//    public Task CreateAsync() => _scene.CreateAsync(_graphics);

//    [JSInvokable]
//    public Task UpdateAsync() => _scene.UpdateAsync(_graphics);
//}

