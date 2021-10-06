namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserLoader //: ILoader
{
    private readonly string _sceneId;
    private readonly IJSRuntime _js;
    private readonly ILoggerFactory _loggerFactory;

    public PhaserLoader(string sceneId, IJSRuntime js, ILoggerFactory loggerFactory)
    {
        _sceneId = sceneId;
        _js = js;
        _loggerFactory = loggerFactory;
    }

    public ValueTask LoadImageAsync(string path)
    {
        throw new NotImplementedException();
    }

    public ValueTask OnProgressChangedAsync(Func<decimal, Task> handler)
    {
        throw new NotImplementedException();
    }
}

