//using System;
//using GameATron4000.Core.Messages;
//using GameATron4000.Core.Services;
//using Microsoft.Extensions.Logging;

//namespace GameATron4000.Core.Engine
//{
//    public class RoomScene : IMessageHandler<RoomChangedEvent>
//    {
//        private readonly IGameScriptInterpreter _gameScriptInterpreter;
//        private readonly IGraphicsEngine _graphicsEngine;
//        private readonly IMediator _mediator;

//        public RoomScene(string roomId, IGameScriptInterpreter gameScriptInterpreter, IGraphicsEngine graphicsEngine,
//            IMediator mediator)
//        {
//            _gameScriptInterpreter = gameScriptInterpreter;
//            _graphicsEngine = graphicsEngine;
//            _mediator = mediator;
//        }

//        public void Handle(RoomChangedEvent message)
//        {
//            _mediator.Publish(new BeforeRoomEnteredEvent(message.RoomId));

//            _currentRoom = _roomSceneFactory.CreateRoomScene(message.RoomId);

//            _mediator.Publish(new AfterRoomEnteredEvent(message.RoomId));
//        }

//    }
//}
