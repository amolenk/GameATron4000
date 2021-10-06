namespace Amolenk.GameATron4000.Engine.Phaser;

public abstract class PhaserScene
{
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

    private readonly string _id;

    private Game? _game;
    private IJSRuntime? _js;
    private ILoggerFactory? _loggerFactory;

    protected GameManifest Manifest => _game.Manifest;

    protected PhaserScene(
        string id,
        Game game,
        IJSRuntime js,
        ILoggerFactory loggerFactory)
    {
        _id = id;
        _game = game;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public async Task InitializeAsync(
        Game game,
        IJSRuntime js,
        ILoggerFactory loggerFactory)
    {
        _game = game;
        _js = js;
        _loggerFactory = loggerFactory;

        var sceneCallbacks = new PhaserSceneCallbacks(
            PreloadAsync,
            CreateAsync,
            UpdateAsync);

        await _js.InvokeVoidAsync(
            PhaserConstants.Functions.RegisterScene,
            _id,
            DotNetObjectReference.Create(sceneCallbacks),
            new ImageAsset[0],
            true);
    }

    protected virtual Task PreloadAsync() => Task.CompletedTask;

    protected virtual Task CreateAsync() => Task.CompletedTask;

    protected virtual Task UpdateAsync() => Task.CompletedTask;

    protected ValueTask AddImageAsync(
        int x,
        int y,
        string texture,
        string? frame = null)
    {
        VerifyInitialized();

        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.AddImage,
            _id,
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

    protected ValueTask AddTextAsync(int x, int y, string text)
    {
        VerifyInitialized();

        return _js!.InvokeVoidAsync("addText", _id, x, y, text);
    }

    protected ValueTask LoadAtlasAsync(
        string key,
        string textureUrl,
        string atlasUrl)
    {
        VerifyInitialized();

        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.LoadAtlas,
            _id,
            key,
            _game!.BasePath + textureUrl,
            _game!.BasePath + atlasUrl);
    }

    protected ValueTask LoadImageAsync(string key, string imageUrl)
    {
        VerifyInitialized();

        return _js!.InvokeVoidAsync(
            PhaserConstants.Functions.LoadImage,
            _id,
            key,
            _game!.BasePath + imageUrl);
    }

    private void VerifyInitialized()
    {
        if (_js == null) throw new InvalidOperationException(
            "Scene not initialized.");
    }
}
