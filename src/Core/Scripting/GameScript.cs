namespace Amolenk.GameATron4000.Scripting;

public class GameScript
{
    private readonly Game _game;
    private readonly EventQueue _eventQueue;
    private GameState _initialState;

    public GameScript(
        Game game,
        EventQueue eventQueue,
        IMediator mediator)
    {
        _game = game;
        _eventQueue = eventQueue;
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

    public Task StartGameAsync()
    {

        // Filter event queue while setting up the game. Only ProtagonistChanged
        // and RoomEntered should be sent to the UI when the game starts.
        //_eventQueue.SetFilter(e => e is ProtagonistChanged || e is RoomEntered);

        // Runs the OnGameStart callback in the game script.
        _game.Start();

        // From this point on, allow all events to go to the UI.
        _eventQueue.AllowAll();

        // We do need to make sure that the startup script has at least set
        // a protagonist and room.
        if (_game.Protagonist is null)
        {
            throw new InvalidOperationException(
                "Startup script must set a protagonist.");
        }
        if (_game.CurrentRoom is null)
        {
            throw new InvalidOperationException(
                "Startup script must enter a room.");
        }

        // Save the initial state so we've got something to compare to when
        // saving the game.
        _initialState = _game.Save();

        return _eventQueue.FlushAsync();
    }

    public Task RestoreGameAsync(GameState gameState)
    {
        _eventQueue.Enqueue(new GameStarted(_game));

        // When restoring a game, no events should go to the UI directly. The
        // player may be in a completely different room than at the start of
        // the game.
        _eventQueue.IgnoreAll();

        // Runs the OnGameStart callback in the game script.
        _game.Start();

        // From this point on, allow all events to go to the UI.
        _eventQueue.AllowAll();

        // Save the initial state so we've got something to compare to when
        // saving the game.
        _initialState = _game.Save();

        // Restore the save game state.
        _game.Restore(gameState);

        // Let the UI know who/where the protagonist is.
        _eventQueue.Enqueue(new ProtagonistChanged(_game.Protagonist));
        _eventQueue.Enqueue(new RoomEntered(_game.CurrentRoom));

        return _eventQueue.FlushAsync();
    }

    public Task ExecutePlayerActionAsync(IAction action)
    {
        _eventQueue.Enqueue(new PlayerActionStarted(action));

        // Move the protagonist to the subject.
        var objectToMoveTo = action.GetObjectToMoveTo();
        if (objectToMoveTo is not null &&
            objectToMoveTo.InteractPosition != RelativePosition.None)
        {
            _game.Protagonist!.MoveTo(objectToMoveTo);
        }

        if (!action.TryExecute())
        {
            _game.Protagonist.SayLine(_game.GetCannedResponse());
        }

        if (!_game.DialogueTreeActive)
        {
            _eventQueue.Enqueue(new PlayerActionCompleted());
        }

        return _eventQueue.FlushAsync();
    }

    public Task ContinueDialogue(DialogueOption option)
    {
        _game.ContinueDialogue(option);

        if (!_game.DialogueTreeActive)
        {
            _eventQueue.Enqueue(new PlayerActionCompleted());
        }

        return _eventQueue.FlushAsync();
    }

    public GameState SaveGame() => _game.Save().GetChanges(_initialState);
}
