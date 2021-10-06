namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserGraphics : IGraphics
{
    private string _sceneId;
    private IJSRuntime _js;
    private ILoggerFactory? _loggerFactory;

    public PhaserGraphics(
        string sceneId,
        IJSRuntime js,
        ILoggerFactory loggerFactory)
    {
        _sceneId = sceneId;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public ValueTask AddImageAsync(
        int x,
        int y,
        string texture,
        string? frame = null) =>
        _js.InvokeVoidAsync(
            PhaserConstants.Functions.AddImage,
            _sceneId,
            x,
            y,
            texture,
            frame);

    //protected async ValueTask<ISprite> AddSpriteAsync(int x, int y, string imageName)
    //{
    //    var id = await _js.InvokeAsync<string>(
    //        "addSprite",
    //        _id,
    //        x,
    //        y,
    //        imageName);

    //    return new PhaserSprite(id, _js, _loggerFactory.CreateLogger<PhaserSprite>());
    //}

    public ValueTask AddTextAsync(int x, int y, string text) =>
        _js.InvokeVoidAsync(
            PhaserConstants.Functions.AddText,
            _sceneId,
            x,
            y,
            text);

    public ValueTask StartSceneAsync(string key) =>
        _js.InvokeVoidAsync(
            PhaserConstants.Functions.StartScene,
            _sceneId, key);
}
