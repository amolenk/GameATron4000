//using Amolenk.GameATron4000.Engine.Messages.Requests;

//namespace Amolenk.GameATron4000.Engine;

//public class Game
//{
//    private readonly GameManifest _manifest;
//    private readonly IDynamicMediator _mediator;

//    public Game(
//        GameManifest manifest,
//        IDynamicMediator mediator)
//    {
//        _manifest = manifest;
//        _mediator = mediator;
//    }

//    public async Task StartAsync()
//    {
//        await _mediator.Publish(new GameStartedEvent(_manifest));

//        var script = new GameScript(_manifest, _mediator);
//        await script.StartAsync();
//    }
//}
