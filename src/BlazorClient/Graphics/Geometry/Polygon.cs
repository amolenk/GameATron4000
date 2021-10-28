namespace Amolenk.GameATron4000.Engine.Graphics.Geometry;

public class Polygon
{
    private readonly List<Point> _vertices;

    public IReadOnlyList<Point> Vertices => _vertices.AsReadOnly();

    public IEnumerable<Line> Edges
    {
        get
        {
            for (var i = 0; i < _vertices.Count; i++)
            {
                var edgeStart = _vertices[i];
                var edgeEnd = _vertices[(i + 1) % _vertices.Count];

                yield return new Line(edgeStart, edgeEnd);
            }
        }
    }

    public Polygon(IEnumerable<Point> vertices)
    {
        _vertices = vertices.ToList();
    }

    // Adapted from https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
    public bool Contains(Point point)
    {
        bool inside = false;
        foreach (var edge in Edges)
        {
            if ((edge.End.Y > point.Y) != (edge.Start.Y > point.Y) &&
                point.X < (edge.Start.X - edge.End.X) * (point.Y - edge.End.Y)
                    / (edge.Start.Y - edge.End.Y) + edge.End.X)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    public bool IsConcaveAt(int index)
    {
        var current = _vertices[index];

        var next = _vertices[(index + 1) % _vertices.Count];
        var previous = _vertices[index == 0 ? _vertices.Count - 1 : index - 1];

        var vector1 = new Vector(current.X - previous.X, current.Y - previous.Y);
        var vector2 = new Vector(next.X - current.X, next.Y - current.Y);

        return vector1.Cross(vector2) < 0;
    }

    public Point? FindClosestPoint(Point point)
    {
        Point? result = null;
        double minDistance = double.MinValue;

        // Not optimal, we only consider the Y-axis.
        foreach (var edge in Edges)
        {
            // Create a vertical line that stops below the edge.
            var testLine = new Line(
                new Point(point.X, 0),
                new Point(point.X, Math.Max(edge.Start.Y, edge.End.Y) + 1));

            // Check where the edge intersects the vertical line.
            if (edge.Intersects2(testLine, out Point intersection))
            {
                var distance = Math.Abs(point.Y - intersection.Y);
                if (minDistance == double.MinValue || minDistance > distance)
                {
                    minDistance = distance;
                    result = intersection;
                }
            }
        }

        return result != null
            ? new Point(result.X, result.Y)
            : null;
    }
}

