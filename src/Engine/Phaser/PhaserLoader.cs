namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserLoader : IAssetLoader
{
    private string _sceneId;
    private string _basePath;
    private IJSRuntime _js;

    public PhaserLoader(
        string sceneId,
        string basePath,
        IJSRuntime js)
    {
        _sceneId = sceneId;
        _basePath = basePath;
        _js = js;
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
            _basePath + textureUrl,
            _basePath + atlasUrl);
    }

    public ValueTask LoadImageAsync(string key, string imageUrl)
    {
        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.LoadImage,
            _sceneId,
            key,
            _basePath + imageUrl);
    }
}
