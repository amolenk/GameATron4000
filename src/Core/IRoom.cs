using System.Collections.Generic;

namespace GameATron4000.Core
{
    public interface IRoom
    {
        string Id { get; }

        IEnumerable<IObject> GetObjects();
    }
}