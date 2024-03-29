﻿namespace Amolenk.GameATron4000.Model;

public class Actor : GameObject
{
    private readonly List<Item> _inventory;

    public string TextColor { get; }

    internal Actor(
        Game game,
        string id,
        ActionHandlers actionHandlers,
        string displayName,
        RelativePosition interactPosition,
        string interactStatus,
        bool isTouchable,
        int scrollFactor,
        int depthOffset,
        string status,
        string textColor)
        : base(
            game,
            id,
            actionHandlers,
            displayName,
            interactPosition,
            interactStatus,
            isTouchable,
            scrollFactor,
            depthOffset,
            status)
    {
        _inventory = new();

        TextColor = textColor;
    }

    public void FaceCamera() => ChangeStatus(WellKnownStatus.FaceCamera);

    public void FaceAwayFromCamera() =>
        ChangeStatus(WellKnownStatus.FaceAwayFromCamera);

    public void AddToInventory(Item item)
    {
        // No need to do anything if this actor already has the item.
        if (_inventory.Contains(item))
        {
            return;
        }

        // If the item is currently in a room, remove it.
        if (Game.TryGetRoomForObject(item, out Room room))
        {
            room.Remove(item);
        }
        // Otherwise, if the item is currently owned by an actor, remove it from
        // their inventory.
        else if (Game.TryGetOwnerForItem(item, out Actor actor))
        {
            actor.RemoveFromInventory(item);
        }

        // Update state.
        _inventory.Add(item);

        Game.EventQueue.Enqueue(new ItemAddedToInventory(item, this));
    }

    public void RemoveFromInventory(Item item)
    {
        if (_inventory.Contains(item))
        {
            _inventory.Remove(item);

            Game.EventQueue.Enqueue(
                new ItemRemovedFromInventory(item, this));
        }
    }

    public bool Has(Item item) => _inventory.Contains(item);

    public void MoveTo(double x, double y, string endInStatus = WellKnownStatus.FaceCamera)
        => MoveTo(new Point(x, y), endInStatus);

    public void MoveTo(
        Point position,
        string endInStatus = WellKnownStatus.FaceCamera)
    {
        if (Game.TryGetRoomForObject(this, out Room room))
        {
            // TODO Also pass ExcludedAreas.
            position = room.Walkbox.SnapToWalkbox(position);
        }

        UpdatePosition(position);
        UpdateStatus(endInStatus);

        Game.EventQueue.Enqueue(new ActorMoved(this, position, endInStatus));
    }

    public void MoveTo(GameObject gameObject)
    {
        if (gameObject.InteractPosition != RelativePosition.None)
        {
            var dY = 0;
            if (gameObject.InteractPosition == RelativePosition.InFront)
            {
                dY = 20;
            }
            else if (gameObject.InteractPosition == RelativePosition.Above)
            {
                dY = -20;
            }

            MoveTo(gameObject.Position.Offset(0, dY), gameObject.InteractStatus);
        }
    }

    public void SayLine(string line)
    {
        Game.EventQueue.Enqueue(new LineSpoken(this, line, Status));
    }

    internal IReadOnlyList<Item> GetInventoryItems() => _inventory.AsReadOnly();

    internal ActorState Save() => new ActorState(
        Position,
        Status,
        _inventory.Select(item => item.Id).ToList());

    internal void Load(ActorState state)
    {
        if (state.Position.HasValue)
        {
            Position = state.Position.Value;
        }

        if (state.Status is not null)
        {
            Status = state.Status;
        }

        if (state.Inventory is not null)
        {
            _inventory.Clear();

            foreach (var id in state.Inventory)
            {
                if (Game.TryGetItem(id, out Item item))
                {
                    _inventory.Add(item);
                }
            }
        }
    }
}
