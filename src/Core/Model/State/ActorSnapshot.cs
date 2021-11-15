namespace Amolenk.GameATron4000.Model.State;

public record ActorSnapshot : GameObjectSnapshot, ISnapshot<ActorSnapshot>
{
    public List<string>? Inventory { get; }

    public ActorSnapshot(
        Point? position,
        string? status,
        List<string> inventory)
        : base(position, status)
    {
        Inventory = inventory;
    }

    public ActorSnapshot? GetChanges(ActorSnapshot baseline)
    {
        var position = Position != baseline.Position ? Position : null;
        var status = Status != baseline.Status ? Status : null;
        var inventory = HasInventoryChanges(baseline.Inventory)
            ? Inventory : null;

        if (position is not null || status is not null || inventory is not null)
        {
            return new ActorSnapshot(position, status, inventory);
        }

        return null;
    }

    private bool HasInventoryChanges(List<string> baseline) =>
        Inventory.Count != baseline.Count || !Inventory.All(baseline.Contains);
}
