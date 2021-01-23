using System;
namespace GameATron4000.Core.Messages
{
    public class AfterRoomEnteredEvent
    {
        public AfterRoomEnteredEvent(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId;
    }
}
