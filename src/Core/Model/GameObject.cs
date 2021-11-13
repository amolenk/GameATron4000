namespace Amolenk.GameATron4000.Model;

public abstract class GameObject
{
    private const string STATE_FRAME = "frame";
    private const string STATE_POSITION = "position";

    public string Id { get; }
    public string DisplayName { get; }
    public string InteractFrameName { get; }
    public RelativePosition InteractPosition { get; }
    public bool IsTouchable { get; }
    public int ScrollFactor { get; }
    public bool IsVisible => GetVisibility();

    public string Frame => StateManager.Get<string>(STATE_FRAME)!;
    public Point Position => StateManager.Get<Point>(STATE_POSITION)!;

    internal ActionHandlers ActionHandlers { get; }

    protected Game Game;
    protected StateManager StateManager { get; }

    protected GameObject(
        Game game,
        string id,
        ActionHandlers actionHandlers,
        string displayName,
        string frame,
        string interactFrameName,
        RelativePosition interactPosition,
        bool isTouchable,
        int scrollFactor)
    {
        Game = game;
        Id = id;
        ActionHandlers = actionHandlers;
        DisplayName = displayName;
        InteractFrameName = interactFrameName;
        InteractPosition = interactPosition;
        IsTouchable = isTouchable;
        ScrollFactor = scrollFactor;

        StateManager = new StateManager();
        StateManager.Set(STATE_FRAME, frame);
        StateManager.Set(STATE_POSITION, new Point(-1, -1));
    }

    public void SetFrame(string frame)
    {
        // Don't need to do anything if the frame stays the same.
        if (StateManager.Get<string>(nameof(Frame)) == frame)
        {
            return;
        }

        // Get the list of visible objects before we change the state.
        var visibleObjectsBefore = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Change the state, this may impact visibility of other objects.
        StateManager.Set(STATE_FRAME, frame);

        // Get the list of visible objects after we change the state.
        var visibleObjectsAfter = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Make lists of the objects that must be hidden/shown in the room.
        var objectsToHide = visibleObjectsBefore?.Except(visibleObjectsAfter!)
            ?? Enumerable.Empty<GameObject>();

        var objectsToShow = visibleObjectsAfter?.Except(visibleObjectsBefore!)
            ?? Enumerable.Empty<GameObject>();

        Game.EventQueue.Enqueue(new GameObjectFrameChanged(
            this,
            frame,
            objectsToHide,
            objectsToShow));
    }

    public override bool Equals(object? obj)
    {
        if (obj is GameObject gameObject)
        {
            return Id.Equals(gameObject.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    internal void SetPosition(Point position) =>
        StateManager.Set(STATE_POSITION, position);

    protected virtual bool GetVisibility() => true;
}
