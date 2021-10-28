namespace Amolenk.GameATron4000.Graphics.Geometry;

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

        var p = new Point(current.X - previous.X, current.Y - previous.Y);
        var q = new Point(next.X - current.X, next.Y - current.Y);

        return (p.X * q.Y - p.Y * q.X) < 0;
    }

    public Point? FindClosestPoint(Point point)
    {
        double minDistance = double.MaxValue;
        Point? closestPoint = null;

        foreach (var edge in Edges)
        {
            var distance = point.DistanceToSegment(edge, out Point intersection);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = intersection;
            }
        }

        return closestPoint;
    }
}

