namespace Amolenk.GameATron4000.Model;

public abstract class GameObject
{
    public string Id { get; }
    public string DisplayName { get; private set; }
    public RelativePosition InteractPosition { get; set; }
    public string InteractStatus { get; set; }
    public bool IsTouchable { get; set; }
    public int ScrollFactor { get; set; }
    public int DepthOffset { get; set; }
    public string Status { get; set; }
    public ActionHandlers When { get; private set; }

    public Point Position { get; protected set; }

    protected Game Game;

    protected GameObject(string id, string displayName, Game game)
    {
        Id = id;
        DisplayName = displayName;
        Game = game;
        InteractPosition = RelativePosition.InFront;
        InteractStatus = WellKnownStatus.FaceCamera;
        IsTouchable = true;
        ScrollFactor = -1;
        DepthOffset = 0;
        Position = new Point(-1, -1);
        Status = WellKnownStatus.Default;
        When = new();
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
        // Only items can have dependencies that change visibility, so we can
        // safely cast the changed objects to Item.
        var objectsToHide = visibleObjectsBefore?.Except(visibleObjectsAfter!)
            ?? Enumerable.Empty<Item>();

        var objectsToShow = visibleObjectsAfter?.Except(visibleObjectsBefore!)
            ?? Enumerable.Empty<Item>();

        Game.EventQueue.Enqueue(new GameObjectStatusChanged(
            this,
            status,
            objectsToHide.ToList(),
            objectsToShow.Select(obj => new GameObjectSnapshot(obj)).ToList()));
    }

    internal void UpdatePosition(Point position) => Position = position;

    internal void UpdateStatus(string status) => Status = status;

    public override bool Equals(object? obj)
    {
        if (obj is GameObject gameObject)
        {
            return Id.Equals(gameObject.Id);
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    // protected void Configure(IGameObjectBuilder builder)
    // {
    //     if (builder.DisplayName.Length > 0)
    //     {
    //         DisplayName = builder.DisplayName;
    //     }
    //     InteractPosition = builder.InteractPosition;
    //     InteractStatus = builder.InteractStatus;
    //     IsTouchable = builder.IsTouchable;
    //     ScrollFactor = builder.ScrollFactor;
    //     DepthOffset = builder.DepthOffset;
    //     Status = builder.Status;
    //     ActionHandlers = builder.When.BuildActionHandlers();
    // }
}
