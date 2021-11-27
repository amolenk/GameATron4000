namespace Amolenk.GameATron4000.Model;

public class Item : GameObject
{
    private ItemDependency? _dependency;

    public bool CanBeUsedWithOtherObjects { get; set; }

    public bool IsVisible
    {
        get
        {
            if (_dependency is not null)
            {
                Console.WriteLine("Has dependency for " + Id + ", result = "
                    + (_dependency.Item is not null));

                Console.WriteLine("Checking dependency for " + Id + ", result = "
                    + (_dependency.Item?.Status == _dependency.Status));

                return _dependency.Item?.Status == _dependency.Status;
            }
            return true;
        }
    }

    public Item(string id, string displayName, Game game)
        : base(id, displayName, game)
    {
    }

    public void DependsOn(Item item, string status)
    {
        _dependency = new ItemDependency(item, status);
    }

    // public void Configure(Action<ItemBuilder> configure)
    // {
    //     ItemBuilder options = new();
    //     configure(options);

    //     base.Configure(options);

    //     _dependency = options.Dependency;
    //     UseWith = options.UseWith;
    // }

    internal ItemState Save() => new ItemState(Position, Status);

    internal void Load(ItemState state)
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
