namespace Amolenk.GameATron4000.Model.State;

public record ItemSnapshot : GameObjectSnapshot, ISnapshot<ItemSnapshot>
{
    public ItemSnapshot(Point? position, string? status)
        : base(position, status)
    {
    }

    public ItemSnapshot? GetChanges(ItemSnapshot baseline)
    {
        var position = Position != baseline.Position ? Position : null;
        var status = Status != baseline.Status ? Status : null;

        if (position is not null || status is not null)
        {
            return new ItemSnapshot(position, status);
        }

        return null;
    }
}