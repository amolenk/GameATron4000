using System;

namespace GameATron4000.Core.Services
{
    public interface IWorldState
    {
        string SelectedActorId { get; }
        string PreviousRoomId { get; }
        string CurrentRoomId { get; }

        void ChangeCurrentRoom(string roomId);
    }
}
