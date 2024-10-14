namespace Amolenk.GameATron4000.Model.Builders;

public class ItemBuilder : GameObjectBuilder<Item, ItemBuilder>
{
    private ItemDependency? _dependency;
    private bool _canBeUsedWithOtherObject;

    internal ItemBuilder(string id, string spriteId, Game game) : base(id, spriteId, game)
    {
    }

    public ItemBuilder CanBeUsedWithOtherObject()
    {
        _canBeUsedWithOtherObject = true;
        return this;
    }

    public ItemBuilder DependsOn(Func<Item> getItem, string status)
    {
        _dependency = new ItemDependency(getItem, status);
        return this;
    }

    public override Item Build() => new Item(
        _game,
        _id,
        _spriteId,
        BuildActionHandlers(),
        _dependency,
        _displayName,
        _interactPosition,
        _interactPositionOffsetX,
        _interactPositionOffsetY,
        _interactStatus,
        _isTouchable,
        _scrollFactor,
        _depthOffset,
        _status,
        _canBeUsedWithOtherObject);
}
