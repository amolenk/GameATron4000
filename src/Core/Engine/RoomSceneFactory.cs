using System;
using GameATron4000.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameATron4000.Core.Engine
{
    public class RoomSceneFactory : IRoomSceneFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RoomSceneFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public RoomScene CreateRoomScene(string roomId)
        {
            return new RoomScene(
                roomId,
                _serviceProvider.GetRequiredService<IGameScriptInterpreter>(),
                _serviceProvider.GetRequiredService<IGraphicsEngine>());
        }
    }
}
