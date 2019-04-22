using System.Collections.Generic;

namespace GameATron4000.Core
{
    public interface IObject
    {
        string Id { get; }

        string Name { get; }

        string[] Classes { get; }

        string Owner { get; }

        string RoomId { get; }

        int? PositionX { get; }

        int? PositionY { get; }

        int? ZOffset { get; }

        string State { get; }

        string UsePosition { get; }

        string UseDirection { get; }
        
        IObjectDependency DependsOn { get; }

        bool IsVisible { get; }
    }
}