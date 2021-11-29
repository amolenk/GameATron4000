namespace Amolenk.GameATron4000.Model.State;

public record RoomState : IState<RoomState>
{
    public List<string>? Objects { get; }

    public RoomState(List<string>? objects)
    {
        Objects = objects;
    }

    public RoomState? GetChanges(RoomState baseline)
    {
        if (HasObjectsChanged(baseline.Objects))
        {
            return new RoomState(Objects);
        }

        return null;
    }

    private bool HasObjectsChanged(List<string>? baseline) =>
        baseline != null &&
        Objects != null &&
        (Objects.Count != baseline.Count || !Objects.All(baseline.Contains));
}
