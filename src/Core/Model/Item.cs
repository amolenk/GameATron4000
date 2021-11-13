namespace Amolenk.GameATron4000.Model;

public class Item : GameObject
{
    private readonly ItemDependency? _dependency;

    public bool CanBeUsedWithOtherObject { get; }

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
            status)
    {
        _dependency = dependency;

        CanBeUsedWithOtherObject = canBeUsedWithOtherObject;
    }

    protected override bool GetVisibility()
    {
        if (_dependency is not null)
        {
            return _dependency.Item.Status == _dependency.Status;
        }

        return base.GetVisibility();
    }
}
