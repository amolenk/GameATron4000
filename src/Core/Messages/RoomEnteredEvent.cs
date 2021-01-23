using System;

namespace GameATron4000.Core.Messages
{
    public class RoomEnteredEvent
    {
        public RoomEnteredEvent(string roomId)
        {
            RoomId = roomId;
        }

        public string RoomId;
    }
}
