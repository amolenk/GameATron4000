using System;
using GameATron4000.Core.Messages;
using GameATron4000.Core.Services;
using Microsoft.Extensions.Logging;

namespace GameATron4000.Core.Domain
{
    public class World
        : IMessageHandler<ChangeCameraFocusCommand>
        , IMessageHandler<BeforeRoomEnteredEvent>
        , IMessageHandler<AfterRoomEnteredEvent>
    {
        private readonly IGameScript _gameScript;
        private readonly IGraphicsEngine _graphicsEngine;
        private readonly ILogger<World> _logger;

        public World(IGameScript gameScript, IGraphicsEngine graphicsEngine, ILogger<World> logger)
        {
            _gameScript = gameScript;
            _graphicsEngine = graphicsEngine;
            _logger = logger;
        }

        public void Handle(ChangeCameraFocusCommand message)
        {
            _logger.LogInformation("Handled command {@Command}.", new
            {
                Name = "ChangeCameraFocusCommand"
            });
        }

        //public void StartGame()
        //{
        //    _gameScript.Initialize();

        //    // And send a GameStarted to the client that contains information on the selected
        //    // actor, inventory items and camera.
        //    await context.SendActivityAsync(activityFactory.GameStarted(script));

        //    // Start the game's first room.
        //    await dc.BeginDialogAsync(result.NextDialogId);

        //    return new World(gameScript, graphicsEngine);
        //}

        public void EnterRoom()
        {
//            _gameScript.BeforeEnterRoom();
//              new Room();
//          _gameScript.AfterEnterRoom();
        }

        public void Handle(BeforeRoomEnteredEvent message)
        {
//            _gameScript.
            throw new NotImplementedException();
        }

        public void Handle(AfterRoomEnteredEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
