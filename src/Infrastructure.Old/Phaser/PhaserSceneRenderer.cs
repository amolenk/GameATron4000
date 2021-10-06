//namespace Amolenk.GameATron4000.Infrastructure.Phaser;

//public class PhaserSceneRenderer : ISceneRenderer
//{
//    private readonly IJSRuntime _js;
//    private readonly PhaserScene _scene;
//    private readonly ILoggerFactory _loggerFactory;

//    public PhaserSceneRenderer(IJSRuntime js, PhaserScene scene, ILoggerFactory loggerFactory)
//    {
//        _js = js;
//        _scene = scene;
//        _loggerFactory = loggerFactory;
//    }

//    [JSInvokable]
//    public Task CreateAsync() => _scene.CreateAsync(this);

//    [JSInvokable]
//    public Task UpdateAsync() => _scene.UpdateAsync(this);

//    public ValueTask AddImageAsync(int x, int y, string name) =>
//        _js.InvokeVoidAsync("addImage", _scene.Id, x, y, name);

//    public async ValueTask<ISprite> AddSpriteAsync(int x, int y, string imageName)
//    {
//        var id = await _js.InvokeAsync<string>(
//            "addSprite",
//            _scene.Id,
//            x,
//            y,
//            imageName);

//        return new PhaserSprite(id, _js, _loggerFactory.CreateLogger<PhaserSprite>());
//    }

//    public ValueTask AddTextAsync(int x, int y, string text) =>
//        _js.InvokeVoidAsync("addText", _scene.Id, x, y, text);
//}

