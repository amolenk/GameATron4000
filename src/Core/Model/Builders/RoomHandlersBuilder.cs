namespace Amolenk.GameATron4000.Model.Builders;

public class RoomHandlersBuilder
{
    private readonly RoomBuilder _roomBuilder;

    public Action? HandleBeforeEnter { get; private set; }
    public Action? HandleAfterEnter { get; private set; }

    internal RoomHandlersBuilder(RoomBuilder roomBuilder)
    {
        _roomBuilder = roomBuilder;
    }

    public RoomBuilder BeforeEnter(Action action)
    {
        HandleBeforeEnter = action;
        return _roomBuilder;
    }

    public RoomBuilder AfterEnter(Action action)
    {
        HandleAfterEnter = action;
        return _roomBuilder;
    }

    public void Build() => new RoomHandlers(
        HandleBeforeEnter,
        HandleAfterEnter);
}
