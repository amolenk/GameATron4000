namespace Amolenk.GameATron4000.Model;

public abstract class GameObject : IGameObject
{
    public string Id { get; }
    public string DisplayName { get; }
    public RelativePosition InteractPosition { get; }
    public string InteractStatus { get; }
    public bool IsTouchable { get; }
    public int ScrollFactor { get; }
    public bool IsVisible => GetVisibility();

    public Point Position { get; protected set; }
    public string Status { get; protected set; }

    public ActionHandlers ActionHandlers { get; }

    protected Game Game;

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
        Position = new Point(-1, -1);
        Status = status;
    }

    public void ChangeStatus(string status)
    {
        // Don't need to do anything if the frame stays the same.
        if (Status == status)
        {
            return;
        }

        // Get the list of visible objects before we change the state.
        var visibleObjectsBefore = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Change the state, this may impact visibility of other objects.
        Status = status;

        // Get the list of visible objects after we change the state.
        var visibleObjectsAfter = Game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Make lists of the objects that must be hidden/shown in the room.
        var objectsToHide = visibleObjectsBefore?.Except(visibleObjectsAfter!)
            ?? Enumerable.Empty<IGameObject>();

        var objectsToShow = visibleObjectsAfter?.Except(visibleObjectsBefore!)
            ?? Enumerable.Empty<IGameObject>();

        Game.EventQueue.Enqueue(new GameObjectStatusChanged(
            this,
            status,
            objectsToHide,
            objectsToShow));
    }

    public void UpdatePosition(Point position) => Position = position;

    public override bool Equals(object? obj)
    {
        if (obj is IGameObject gameObject)
        {
            return Id.Equals(gameObject.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    protected virtual bool GetVisibility() => true;
}
