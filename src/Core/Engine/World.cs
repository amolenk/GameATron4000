//using System;
//using GameATron4000.Core.Domain;
//using GameATron4000.Core.Messages;
//using GameATron4000.Core.Services;
//using Microsoft.Extensions.Logging;

//namespace GameATron4000.Core.Engine
//{
//    public class World : IMessageHandler<RoomChangedEvent>
//    {
//        private readonly IGameScriptInterpreter _gameScriptInterpreter;
//        private readonly IGraphicsEngine _graphicsEngine;
//        private readonly IRoomSceneFactory _roomSceneFactory;
//        private readonly IMediator _mediator;
//        private readonly ILogger<World> _logger;

//        private RoomScene? _currentRoom;

//        public World(IGameScriptInterpreter gameScriptInterpreter, IGraphicsEngine graphicsEngine,
//            IRoomSceneFactory roomSceneFactory, IMediator mediator, ILogger<World> logger)
//        {
//            _gameScriptInterpreter = gameScriptInterpreter;
//            _graphicsEngine = graphicsEngine;
//            _roomSceneFactory = roomSceneFactory;
//            _mediator = mediator;
//            _logger = logger;
//        }

//        public void Handle(RoomChangedEvent message)
//        {
//            _mediator.Publish(new BeforeRoomEnteredEvent(message.RoomId));

//            _currentRoom = _roomSceneFactory.CreateRoomScene(message.RoomId);

//            _mediator.Publish(new AfterRoomEnteredEvent(message.RoomId));
//        }
//    }
//}
