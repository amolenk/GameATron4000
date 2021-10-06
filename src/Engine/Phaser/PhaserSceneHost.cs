namespace Amolenk.GameATron4000.Engine.Phaser;

public class PhaserSceneHost : ISceneHost
{
    private readonly string _containerElementId;
    private readonly GameManifest _manifest;
    private readonly IJSRuntime _js;
    private readonly ILoggerFactory _loggerFactory;

    public PhaserSceneHost(
        string containerElementId,
        GameManifest manifest,
        IJSRuntime js,
        ILoggerFactory loggerFactory)
    {
        _containerElementId = containerElementId;
        _manifest = manifest;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public async ValueTask RegisterSceneAsync<TScene>(TScene scene)
        where TScene : Scene
    {
        var sceneWrapper = new PhaserSceneWrapper<TScene>(
            scene,
            _manifest.BasePath,
            _js,
            _loggerFactory);

        await _js.InvokeVoidAsync(
            PhaserConstants.Functions.RegisterScene,
            scene.Id,
            DotNetObjectReference.Create(sceneWrapper),
            true);
    }

    public async ValueTask SwitchToSceneAsync(string key)
    {

    }

    public async ValueTask StartAsync()
    {
        await _js.InvokeVoidAsync(
            PhaserConstants.Functions.StartPhaser,
            _containerElementId);
    }
}

public class PhaserHost
{
    private const string SCENE_ID = "main";

    private readonly GameManifest _manifest;
    private readonly IJSRuntime _js;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private readonly IRequest _onCreateCommand;

    public PhaserHost(
        GameManifest manifest,
        IJSRuntime js,
        ILoggerFactory loggerFactory,
        IMediator mediator,
        Func<IRequest> onCreateCommandFactory)
    {
        _manifest = manifest;
        _js = js;
        _loggerFactory = loggerFactory;
        _mediator = mediator;

        var loader = new PhaserLoader(SCENE_ID, manifest.BasePath, js);
        var graphics = new PhaserGraphics(SCENE_ID, js, loggerFactory);

        _onCreateCommand = onCreateCommandFactory();
    }

    public IGraphics Graphics
    {
        get
        {
            return new PhaserGraphics(SCENE_ID, _js, _loggerFactory);
        }
    }

    public async ValueTask StartAsync(string containerElementId)
    {
        //await _js.InvokeVoidAsync(
        //    PhaserConstants.Functions.RegisterScene,
        //    SCENE_ID,
        //    DotNetObjectReference.Create(this),
        //    true);

        await _js.InvokeVoidAsync(
            PhaserConstants.Functions.StartPhaser,
            containerElementId,
            DotNetObjectReference.Create(this));
    }

    [JSInvokable]
    public Task PreloadAsync()
    {
        List<Task> loadTasks = new();

        var loader = new PhaserLoader(SCENE_ID, _manifest.BasePath, _js);

        foreach (var imageSpec in _manifest.Spec.Images)
        {
            loadTasks.Add(
                loader.LoadImageAsync(imageSpec.Key, imageSpec.ImageUrl)
                .AsTask());
        }

        foreach (var atlasSpec in _manifest.Spec.Atlases)
        {
            loadTasks.Add(
                loader.LoadAtlasAsync(
                    atlasSpec.Key,
                    atlasSpec.TextureUrl,
                    atlasSpec.AtlasUrl)
                .AsTask());
        }

        return Task.WhenAll(loadTasks);
    }

    [JSInvokable]
    public Task CreateAsync() => _mediator.Send(_onCreateCommand);

    [JSInvokable]
    public Task UpdateAsync() => Task.CompletedTask;
}
