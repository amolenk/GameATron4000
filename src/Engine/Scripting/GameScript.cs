namespace Amolenk.GameATron4000.Engine.Scripting;

public class GameScript
    : IRequestHandler<RunStartScriptCommand>
//    : INotificationHandler<RoomViewReadyEvent>
{
    private readonly GameManifest _manifest;
    private readonly IDynamicMediator _mediator;

    // TODO Move params to InitializeAsync, or create a static Create/Init method
    public GameScript(GameManifest manifest, IDynamicMediator mediator)
    {
        _manifest = manifest;
        _mediator = mediator;
    }

    public Task InitializeAsync()
    {
        _mediator.Subscribe<RunStartScriptCommand>(this);
//        _mediator.Subscribe<RoomViewReadyEvent>(this);

        return Task.CompletedTask;
    }

    //public async Task Handle(RoomViewReadyEvent notification, CancellationToken cancellationToken)
    //{
    //    await _mediator.Publish(new RoomEnteredEvent());
    //}

    public async Task<Unit> Handle(RunStartScriptCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new RoomEnteredEvent());

        return Unit.Value;
    }
}
