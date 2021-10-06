namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public class PhaserSprite : ISprite
{
    private readonly string _id;
    private readonly IJSRuntime _js;
    private readonly ILogger _logger;

    private Dictionary<string, Func<Task>> _eventHandlers = new();

    public PhaserSprite(string id, IJSRuntime js, ILogger<PhaserSprite> logger)
    {
        _id = id;
        _js = js;
        _logger = logger;
    }

    public ValueTask SetAnchorAsync(double value)
    {
        _logger.LogWarning(
            "WARNING PhaserSprite.SetAnchorAsync is not implemented");

        return ValueTask.CompletedTask;
    }

    public async ValueTask OnPointerDownAsync(Func<Task> handler)
    {
        _eventHandlers[PhaserConstants.Input.Events.PointerDown] = handler;

        await _js.InvokeVoidAsync(
            PhaserConstants.Functions.SetSpriteInteraction,
            _id,
            DotNetObjectReference.Create(this),
            PhaserConstants.Input.Events.PointerDown);
    }

    [JSInvokable]
    public async ValueTask OnInputAsync(string eventName)
    {
        if (_eventHandlers.TryGetValue(eventName, out var handler))
        {
            await handler();
            return;
        }

        _logger.LogError(
            "Cannot find handler for sprite event {Sprite}.{Event}.",
            _id,
            eventName);
    }
}
