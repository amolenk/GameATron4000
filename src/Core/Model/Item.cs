namespace Amolenk.GameATron4000.Model;

public class Item : GameObject
{
    private readonly ItemDependency? _dependency;

    public bool CanBeUsedWithOtherObject { get; }

    public bool IsVisible
    {
        get
        {
            if (_dependency is not null)
            {
                return _dependency.Item?.Status == _dependency.Status;
            }
            return true;
        }
    }

    public Item(
        Game game,
        string id,
        ActionHandlers actionHandlers,
        ItemDependency? dependency,
        string displayName,
        RelativePosition interactPosition,
        string interactStatus,
        bool isTouchable,
        int scrollFactor,
        int depthOffset,
        string status,
        bool canBeUsedWithOtherObject)
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
        _dependency = dependency;

        CanBeUsedWithOtherObject = canBeUsedWithOtherObject;
    }

    internal ItemState Save() => new ItemState(Position, Status);

    internal void Restore(ItemState state)
    {
        if (state.Position.HasValue)
        {
            Position = state.Position.Value;
        }

        if (state.Status is not null)
        {
            Status = state.Status;
        }
    }
}
