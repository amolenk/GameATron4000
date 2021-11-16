namespace Amolenk.GameATron4000.Model.State;

public record ItemState : GameObjectState, IState<ItemState>
{
    public ItemState(Point? position, string? status)
        : base(position, status)
    {
    }

    public ItemState? GetChanges(ItemState baseline)
    {
        var position = Position != baseline.Position ? Position : null;
        var status = Status != baseline.Status ? Status : null;

        if (position is not null || status is not null)
        {
            return new ItemState(position, status);
        }

        return null;
    }
}