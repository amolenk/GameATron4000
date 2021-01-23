using System;

namespace GameATron4000.Core.Services
{
    public interface IItemState
    {
        string Owner { get; }
        string RoomId { get; }

        void ChangeOwner(string owner);

        void ChangePosition(string roomId, int x, int y);
    }
}
