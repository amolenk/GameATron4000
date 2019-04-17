using System.Collections.Generic;

namespace GameATron4000.Core
{
    public interface IActor
    {
        string Id { get; }

        string Name { get; }

        string[] Classes { get; }

        string TextColor { get; }

        string RoomId { get; }

        int? PositionX { get; }

        int? PositionY { get; }

        IEnumerable<IObject> GetInventory();
    }
}