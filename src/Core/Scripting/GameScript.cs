namespace Amolenk.GameATron4000.Scripting;

// TODO Make disposable and unsubscribe in Dispose
public class GameScript : IGameScriptApi
    , IRequestHandler<StartGame>
{
    public List<Room> Rooms { get; } = new();

    private readonly ICustomMediator _mediator;

    public GameScript(ICustomMediator mediator)
    {
        _mediator = mediator;
        _mediator.Subscribe<StartGame>(this);
    }

    public Room AddRoom(string id, Walkbox walkbox)
    {
        var room = new Room(id, walkbox);
        Rooms.Add(room);

        return room;
    }

    public async Task<Unit> Handle(StartGame notification, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new RoomEntered(
            Rooms.First(),
            new[]
            {
                new ActorTemp
                {
                    Id = "al",
                    PositionX = 560,
                    PositionY = 400
                }
            }));

        return Unit.Value;
    }
}
