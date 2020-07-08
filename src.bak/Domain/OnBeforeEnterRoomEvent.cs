using MediatR;

namespace GameATron4000.Domain
{
    public class OnBeforeEnterRoomCommand : IRequest
    {
        public OnBeforeEnterRoomCommand(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; }
    }
}