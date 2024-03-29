﻿namespace Amolenk.GameATron4000.Model;

public abstract class GameObject
{
    public string Id { get; }
    public string DisplayName { get; }
    public RelativePosition InteractPosition { get; }
    public string InteractStatus { get; }
    public bool IsTouchable { get; }
    public int ScrollFactor { get; }
    public int DepthOffset { get; }

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
        int depthOffset,
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
        DepthOffset = depthOffset;
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
}
