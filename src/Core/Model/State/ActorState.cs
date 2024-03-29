namespace Amolenk.GameATron4000.Model.State;

public record ActorState : GameObjectState, IState<ActorState>
{
    public List<string>? Inventory { get; }

    public ActorState(
        Point? position,
        string? status,
        List<string>? inventory)
        : base(position, status)
    {
        Inventory = inventory;
    }

    public ActorState? GetChanges(ActorState baseline)
    {
        var position = Position != baseline.Position ? Position : null;
        var status = Status != baseline.Status ? Status : null;
        var inventory = HasInventoryChanges(baseline.Inventory)
            ? Inventory : null;

        if (position is not null || status is not null || inventory is not null)
        {
            return new ActorState(position, status, inventory);
        }

        return null;
    }

    private bool HasInventoryChanges(List<string>? baseline) =>
        baseline is not null &&
        Inventory is not null &&
        (Inventory.Count != baseline.Count || !Inventory.All(baseline.Contains));
}
