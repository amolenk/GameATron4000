namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserGraphics
{
    private Game _game;
    private string _sceneId;
    private IJSRuntime _js;
    private ILoggerFactory? _loggerFactory;

    public PhaserGraphics(
        Game game,
        string sceneId,
        IJSRuntime js,
        ILoggerFactory loggerFactory)
    {
        _game = game;
        _sceneId = sceneId;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public ValueTask AddImageAsync(
        int x,
        int y,
        string texture,
        string? frame = null)
    {
        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.AddImage,
            _sceneId,
            x,
            y,
            texture,
            frame);
    }

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

    public ValueTask AddTextAsync(int x, int y, string text)
    {
        return _js!.InvokeVoidAsync("addText", _sceneId, x, y, text);
    }

    public ValueTask LoadAtlasAsync(
        string key,
        string textureUrl,
        string atlasUrl)
    {
        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.LoadAtlas,
            _sceneId,
            key,
            _game.BasePath + textureUrl,
            _game.BasePath + atlasUrl);
    }

    public ValueTask LoadImageAsync(string key, string imageUrl)
    {
        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.LoadImage,
            _sceneId,
            key,
            _game.BasePath + imageUrl);
    }
}
