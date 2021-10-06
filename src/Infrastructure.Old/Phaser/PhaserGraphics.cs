namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserGraphics //: IGraphics
{
    private readonly string _sceneId;
    private readonly IJSRuntime _js;
    private readonly ILoggerFactory _loggerFactory;

    public PhaserGraphics(string sceneId, IJSRuntime js, ILoggerFactory loggerFactory)
    {
        _sceneId = sceneId;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public ValueTask AddImageAsync(int x, int y, string name) =>
        _js.InvokeVoidAsync("addImage", _sceneId, x, y, name);

    public async ValueTask<ISprite> AddSpriteAsync(int x, int y, string imageName)
    {
        var id = await _js.InvokeAsync<string>(
            "addSprite",
            _sceneId,
            x,
            y,
            imageName);

        return new PhaserSprite(id, _js, _loggerFactory.CreateLogger<PhaserSprite>());
    }

    public ValueTask AddTextAsync(int x, int y, string text) =>
        _js.InvokeVoidAsync("addText", _sceneId, x, y, text);
}

