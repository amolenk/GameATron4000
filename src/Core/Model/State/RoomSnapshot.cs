namespace Amolenk.GameATron4000.Model.State;

public record RoomSnapshot : ISnapshot<RoomSnapshot>
{
    public List<string>? Objects { get; }

    public RoomSnapshot(List<string>? objects)
    {
        Objects = objects;
    }

    public RoomSnapshot? GetChanges(RoomSnapshot baseline)
    {
        if (HasObjectsChanged(baseline.Objects))
        {
            return new RoomSnapshot(Objects);
        }

        return null;
    }

    private bool HasObjectsChanged(List<string> baseline) =>
        Objects.Count != baseline.Count || !Objects.All(baseline.Contains);
}
