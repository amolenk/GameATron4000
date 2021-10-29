namespace Amolenk.GameATron4000.Scripting;

// TODO Make disposable and unsubscribe in Dispose
public class GameScript : IRequestHandler<StartGame>
{
    private readonly Game _game;
    private readonly NotificationCollector _notificationCollector;
    private readonly ICustomMediator _mediator;

    public GameScript(
        Game game,
        NotificationCollector notificationCollector,
        ICustomMediator mediator)
    {
        _game = game;
        _notificationCollector = notificationCollector;
        _mediator = mediator;

        _mediator.Subscribe<StartGame>(this);
    }

    public static async Task<GameScript> LoadAsync(
        IEnumerable<ScriptFile> sources,
        IScriptCompiler compiler,
        ICustomMediator mediator)
    {
        // TODO Dispose ScriptRunner
        var scriptRunner = compiler.Compile<Game>(sources);
        var notificationCollector = new NotificationCollector();

        var game = new Game(notificationCollector);

        await scriptRunner.RunAsync(game);

        return new GameScript(game, notificationCollector, mediator);
    }

    public async Task<Unit> Handle(
        StartGame notification,
        CancellationToken cancellationToken)
    {
        _game.Start();

        // TODO Verify that CurrentRoom and SelectedActor are both set.

        await _mediator.Publish(new ProtagonistChanged(_game.Protagonist));
        await _mediator.Publish(new RoomEntered(_game.CurrentRoom));

        return Unit.Value;
    }
}
