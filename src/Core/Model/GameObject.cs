namespace Amolenk.GameATron4000.Model;

public abstract class GameObject
{
    private const string STATE_POSITION = "position";
    private const string STATE_STATUS = "status";

    public string Id { get; }
    public string DisplayName { get; }
    public RelativePosition InteractPosition { get; }
    public string InteractStatus { get; }
    public bool IsTouchable { get; }
    public int ScrollFactor { get; }
    public bool IsVisible => GetVisibility();

    public Point Position => StateManager.Get<Point>(STATE_POSITION)!;
    public string Status => StateManager.Get<string>(STATE_STATUS)!;

    public ActionHandlers ActionHandlers { get; }

    protected Game Game;
    protected StateManager StateManager { get; }

    protected GameObject(
        Game game,
        string id,
        ActionHandlers actionHandlers,
        string displayName,
        RelativePosition interactPosition,
        string interactStatus,
        bool isTouchable,
        int scrollFactor,
        string status)
    {
        Game = game;
        Id = id;
        ActionHandlers = actionHandlers;
        DisplayName = displayName;
        InteractPosition = interactPosition;
        InteractStatus = interactStatus;
        IsTouchable = isTouchable;
        ScrollFactor = scrollFactor;

        StateManager = new StateManager();
        StateManager.Set(STATE_POSITION, new Point(-1, -1));
        StateManager.Set(STATE_STATUS, status);
    }

    public void ChangeStatus(string status)
    {
        // Don't need to do anything if the frame stays the same.
        if (StateManager.Get<string>(STATE_STATUS) == status)
        {
            return;
        }

        // Get the list of visible objects before we change the state.
        var visibleObjectsBefore = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Change the state, this may impact visibility of other objects.
        StateManager.Set(STATE_STATUS, status);

        // Get the list of visible objects after we change the state.
        var visibleObjectsAfter = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Make lists of the objects that must be hidden/shown in the room.
        var objectsToHide = visibleObjectsBefore?.Except(visibleObjectsAfter!)
            ?? Enumerable.Empty<GameObject>();

        var objectsToShow = visibleObjectsAfter?.Except(visibleObjectsBefore!)
            ?? Enumerable.Empty<GameObject>();

        Game.EventQueue.Enqueue(new GameObjectStatusChanged(
            this,
            status,
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
