namespace Amolenk.GameATron4000.Model.Builders;

public class ItemBuilder : GameObjectBuilder<Item, ItemBuilder>
{
    private ItemDependency? _dependency;
    private bool _canBeUsedWithOtherObject;

    internal ItemBuilder(string id, Game game) : base(id, game)
    {
    }

    public ItemBuilder CanBeUsedWithOtherObject()
    {
        _canBeUsedWithOtherObject = true;
        return this;
    }

    public ItemBuilder DependsOn(Item item, string frame)
    {
        _dependency = new ItemDependency(item, frame);
        return this;
    }

    public override Item Build() => new Item(
        _game,
        _id,
        BuildActionHandlers(),
        _dependency,
        _displayName,
        _frame,
        _interactFrame,
        _interactPosition,
        _isTouchable,
        _scrollFactor,
        _canBeUsedWithOtherObject);
}
