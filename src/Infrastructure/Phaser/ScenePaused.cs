namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class ScenePaused : IDisposable
{
    private IJSInProcessRuntime _jsRuntime;

    public ScenePaused(IJSInProcessRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void Dispose() =>
        _jsRuntime.InvokeVoid(PhaserConstants.Functions.Resume);
}
