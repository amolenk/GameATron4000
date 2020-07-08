using MediatR;

namespace GameATron4000.Domain
{
    public class OnAfterEnterRoomCommand : IRequest
    {
        public OnAfterEnterRoomCommand(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId { get; }
    }
}