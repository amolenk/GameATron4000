using System;

namespace GameATron4000.Core.Messages
{
    public class BeforeRoomEnteredEvent
    {
        public BeforeRoomEnteredEvent(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId;
    }
}
