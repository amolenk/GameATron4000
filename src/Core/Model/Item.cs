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
        string frame,
        string interactFrameName,
        RelativePosition interactPosition,
        bool isTouchable,
        int scrollFactor,
        bool canBeUsedWithOtherObject)
        : base(
            game,
            id,
            actionHandlers,
            displayName,
            frame,
            interactFrameName,
            interactPosition,
            isTouchable,
            scrollFactor)
    {
        _dependency = dependency;

        CanBeUsedWithOtherObject = canBeUsedWithOtherObject;
    }

    protected override bool GetVisibility()
    {
        if (_dependency is not null)
        {
            return _dependency.Item.Frame == _dependency.Frame;
        }

        return base.GetVisibility();
    }
}
