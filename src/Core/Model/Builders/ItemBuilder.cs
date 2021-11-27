namespace Amolenk.GameATron4000.Model.Builders;

public class ItemBuilder : GameObjectBuilder<Item, ItemBuilder>
{
    public ItemDependency? Dependency { get; private set; }
    public bool UseWith { get; private set; }

    internal ItemBuilder() : base()
    {
    }

    public ItemBuilder CanBeUsedWithOtherObject()
    {
        UseWith = true;
        return this;
    }

    public ItemBuilder DependsOn(Item item, string status)
    {
        Dependency = new ItemDependency(item, status);
        return this;
    }
}
