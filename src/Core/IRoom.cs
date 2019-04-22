using System.Collections.Generic;
using System.Drawing;

namespace GameATron4000.Core
{
    public interface IRoom
    {
        string Id { get; }

        IEnumerable<Point> Walkbox { get; }

        IEnumerable<IObject> GetObjects();
    }
}