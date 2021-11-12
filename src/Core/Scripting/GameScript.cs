namespace Amolenk.GameATron4000.Scripting;

// TODO Make disposable and unsubscribe in Dispose
public class GameScript
{
    private readonly Game _game;
    private readonly EventQueue _eventQueue;

    public GameScript(
        Game game,
        EventQueue eventQueue,
        IMediator mediator)
    {
        _game = game;
        _eventQueue = eventQueue;

        mediator.Subscribe<StartGame>(OnStartGameAsync);
        mediator.Subscribe<ExecutePlayerAction>(OnExecutePlayerActionAsync);
    }

    public static async Task<GameScript> LoadAsync(
        IEnumerable<ScriptFile> sources,
        IScriptCompiler compiler,
        IMediator mediator)
    {
        // TODO Dispose ScriptRunner
        var scriptRunner = compiler.Compile<Game>(sources);

        EventQueue eventQueue = new(mediator);

        var game = new Game(eventQueue);

        await scriptRunner.RunAsync(game);

        return new GameScript(game, eventQueue, mediator);
    }

    private Task OnStartGameAsync(StartGame _)
    {
        // Disable event queue while setting up the game. No action events
        // should be sent to the UI yet.
        _eventQueue.IgnoreNewEvents = true;

        // Runs the OnGameStart callback in the game script.
        _game.Start();

        _eventQueue.IgnoreNewEvents = false;

        // We do need to make sure that the startup script has at least set
        // a protagonist and room.
        if (_game.State.Protagonist is null)
        {
            throw new InvalidOperationException(
                "Startup script must set a protagonist.");
        }
        if (_game.CurrentRoom is null)
        {
            throw new InvalidOperationException(
                "Startup script must enter a room.");
        }

        _eventQueue.Enqueue(new ProtagonistChanged(
            _game.State.Protagonist));

        _eventQueue.Enqueue(new RoomEntered(
            _game.CurrentRoom,
            _game.CurrentRoom.GetVisibleObjects().ToList()));

        return _eventQueue.FlushAsync();
    }

    private Task OnExecutePlayerActionAsync(ExecutePlayerAction command)
    {
        var action = command.Action;

        _eventQueue.Enqueue(new PlayerActionStarted(action));

        // TODO Also check if subject isn't in inventory
        // Move the protagonist to the subject.
        var interactWithObject = action.GetInteractObject();
        if (interactWithObject is not null)
        {
            _game.State.Protagonist!.MoveTo(interactWithObject);
        }

        // TODO TryExec
        action.Execute();

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

        _eventQueue.Enqueue(new PlayerActionCompleted(action));

        return _eventQueue.FlushAsync();
    }
}
