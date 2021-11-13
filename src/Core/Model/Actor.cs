namespace Amolenk.GameATron4000.Model;

public class Actor : GameObject
{
    private const string STATE_INVENTORY = "inventory";

    public string TextColor { get; }

    public IEnumerable<Item> Inventory =>
        StateManager.Get<List<Item>>(STATE_INVENTORY)!;

    internal Actor(
        Game game,
        string id,
        ActionHandlers actionHandlers,
        string displayName,
        RelativePosition interactPosition,
        string interactStatus,
        bool isTouchable,
        int scrollFactor,
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
            status)
    {
        TextColor = textColor;

        StateManager.Set(STATE_INVENTORY, new List<Item>());
    }

    public void FaceCamera() => ChangeStatus(WellKnownStatus.FaceCamera);

    public void FaceAwayFromCamera() =>
        ChangeStatus(WellKnownStatus.FaceAwayFromCamera);

    public void AddToInventory(Item item)
    {
        var inventory = StateManager.Get<List<Item>>(STATE_INVENTORY)!;

        // No need to do anything if this actor already has the item.
        if (inventory.Contains(item))
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
        inventory.Add(item);

        Game.EventQueue.Enqueue(new ItemAddedToInventory(item, this));
    }

    public void RemoveFromInventory(Item item)
    {
        var inventory = StateManager.Get<List<Item>>(STATE_INVENTORY)!;

        if (inventory.Contains(item))
        {
            inventory.Remove(item);

            Game.EventQueue.Enqueue(
                new ItemRemovedFromInventory(item, this));
        }
    }

    public bool Has(Item item) => Inventory.Contains(item);

    public void MoveTo(Point position)
    {
        if (Game.TryGetRoomForObject(this, out Room room))
        {
            // TODO Also pass ExcludedAreas.
            position = room.Walkbox.SnapToWalkbox(position);
        }

        SetPosition(position);

        Game.EventQueue.Enqueue(new ActorMoved(this, position));
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

            MoveTo(gameObject.Position.Offset(0, dY));
        }
    }

    public void SayLine(string line)
    {
        Game.EventQueue.Enqueue(new LineSpoken(this, line));
    }
}
