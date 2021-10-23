namespace Amolenk.GameATron4000.Engine.Scripting;

// TODO Make disposable and unsubscribe in Dispose
public class GameScript : IGameScriptApi
    , IRequestHandler<StartGameCommand>
{
    public List<RoomScript> Rooms { get; } = new();

    private readonly ICustomMediator _mediator;

    public GameScript(ICustomMediator mediator)
    {
        _mediator = mediator;
        _mediator.Subscribe<StartGameCommand>(this);
    }

    public RoomScript AddRoom(string id)
    {
        var room = new RoomScript();
        Rooms.Add(room);

        return room;
    }

    public async Task<Unit> Handle(StartGameCommand notification, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new RoomEnteredEvent(
            "park",
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
