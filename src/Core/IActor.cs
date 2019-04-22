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

        int? PositionX { get; set; }

        int? PositionY { get; set; }

        string UsePosition { get; }

        string UseDirection { get; }

        string FaceDirection { get; set; }

        IEnumerable<IObject> GetInventory();
    }
}