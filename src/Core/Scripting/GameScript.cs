namespace Amolenk.GameATron4000.Scripting;

// TODO Make disposable and unsubscribe in Dispose
public class GameScript
{
    private readonly Game _game;
    private readonly List<IEvent> _actionEvents;
    private readonly IMediator _mediator;

    public GameScript(
        Game game,
        List<IEvent> actionEvents,
        IMediator mediator)
    {
        _game = game;
        _actionEvents = actionEvents;
        _mediator = mediator;

        _mediator.Subscribe<StartGame>(OnStartGame);
        _mediator.Subscribe<ExecutePlayerAction>(OnExecutePlayerAction);
    }

    public static async Task<GameScript> LoadAsync(
        IEnumerable<ScriptFile> sources,
        IScriptCompiler compiler,
        IMediator mediator)
    {
        // TODO Dispose ScriptRunner
        var scriptRunner = compiler.Compile<Game>(sources);

        List<IEvent> playerActionEvents = new();

        var game = new Game(new Collector<IEvent>(playerActionEvents));

        await scriptRunner.RunAsync(game);

        return new GameScript(game, playerActionEvents, mediator);
    }

    private async Task OnStartGame(StartGame _)
    {
        _game.Start();

        // TODO Verify that CurrentRoom and SelectedActor are both set.

        await _mediator.PublishAsync(new ProtagonistChanged(_game.Protagonist));
        await _mediator.PublishAsync(new RoomEntered(_game.CurrentRoom));
    }

    private async Task OnExecutePlayerAction(ExecutePlayerAction command)
    {
        var action = command.Action;

        await _mediator.PublishAsync(new PlayerActionStarted(action));

        _actionEvents.Clear();

        // TODO Walk to subject

        // TODO TryExec
        action.Execute(action.Subject!.ActionHandlers);

        foreach (var @event in _actionEvents)
        {
            await _mediator.PublishAsync(@event);
        }

        // var handler = subject.CommandHandlers[verb];
        // if (handler != null)
        // {
        //     handler.Invoke()
        // }
        // else
        // {
        //     // TODO
        //     _game.SayLine("[STANDARD RESPONSE]");
        // }

        await _mediator.PublishAsync(new PlayerActionCompleted(action));
    }
}
