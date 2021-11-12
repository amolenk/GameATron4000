namespace Amolenk.GameATron4000.Model;

public class GameObject
{
    private readonly Game _game;
    private readonly GameObjectCondition _condition;

    public string Id { get; }
    public string DisplayName { get; }
    public bool IsTouchable { get; }
    public bool UseWith { get; }
    public string InteractFrameName { get; }
    public RelativePosition InteractPosition { get; }
    public int ScrollFactor { get; }

    public bool IsVisible => _condition?.IsTrue ?? true;
    public string Frame => State.Frame;
    public Point Position => State.Position;
    public Room? Room => State.Room;
    public Actor? Owner => State.Owner;

    internal GameObjectHandlers Handlers { get; }

    protected EventQueue EventQueue { get; }
    protected GameObjectState State { get; }

    internal GameObject(IGameObjectBuilder builder)
    {
        _game = builder.Game;
        _condition = builder.Condition;

        Id = builder.Id;
        DisplayName = builder.DisplayName;
        IsTouchable = builder.IsTouchable;
        UseWith = builder.UseWith;
        InteractFrameName = builder.InteractFrameName;
        InteractPosition = builder.InteractPosition;
        ScrollFactor = builder.ScrollFactor;
        Handlers = new(builder.HandlersBuilder);
        EventQueue = builder.Game.EventQueue;

        State = new()
        {
            Frame = builder.FrameName
        };
    }

    public void SetOwner(Actor newOwner)
    {
        // Don't need to do anything if the owner stays the same.
        if (State.Owner == newOwner)
        {
            return;
        }

        // If the object is currently placed in a room, remove it.
        if (State.Room is not null)
        {
            State.Room.Remove(this);
        }

        // If the object is currently owned by somebody else, take it away.
        if (State.Owner is not null)
        {
            ClearOwner();
        }

        // Set the new owner.
        State.Owner = newOwner;

        EventQueue.Enqueue(new GameObjectAddedToInventory(
            this,
            newOwner));
    }

    public void ClearOwner()
    {
        if (State.Owner is not null)
        {
            var previousOwner = State.Owner;
            State.Owner = null;

            EventQueue.Enqueue(new GameObjectRemovedFromInventory(
                this,
                previousOwner));
        }
    }

    public void SetFrame(string frameName)
    {
        // Don't need to do anything if the frame stays the same.
        if (State.Frame == frameName)
        {
            return;
        }

        // Get the list of visible objects before we change the state.
        var visibleObjectsBefore = _game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Change the state, this may impact visibility of other objects.
        State.Frame = frameName;

        // Get the list of visible objects after we change the state.
        var visibleObjectsAfter = _game.CurrentRoom?
            .GetVisibleObjects().ToList();

        // Make lists of the objects that must be hidden/shown in the room.
        var objectsToHide = visibleObjectsBefore?.Except(visibleObjectsAfter!)
            ?? Enumerable.Empty<GameObject>();

        var objectsToShow = visibleObjectsAfter?.Except(visibleObjectsBefore!)
            ?? Enumerable.Empty<GameObject>();

        EventQueue.Enqueue(new GameObjectFrameChanged(
            this,
            frameName,
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

    internal void NotifyPlacedInRoom(Room room, Point position)
    {
        State.Room = room;
        State.Position = position;
    }
}
