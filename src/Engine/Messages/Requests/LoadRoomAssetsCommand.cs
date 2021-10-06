using System;
namespace Amolenk.GameATron4000.Engine.Messages.Requests
{
    public record LoadRoomAssetsCommand(IAssetLoader Loader) : IRequest
    {
    }
}

