namespace Amolenk.GameATron4000.Graphics.Geometry;

public record Point(double X, double Y)
{
    public Point Offset(double dX, double dY) => new Point(X + dX, Y + dY);

    public static double DistanceBetween(Point p1, Point p2)
    {
        var distX = p2.X - p1.X;
        var distY = p2.Y - p1.Y;

        return Math.Sqrt(distX * distX + distY * distY);
    }

    public double DistanceToSegment(Line segment) =>
        DistanceToSegment(segment, out Point _);
    
    // Adapted from http://paulbourke.net/geometry/pointlineplane/DistancePoint.java
    public double DistanceToSegment(Line segment, out Point intersection)
    {
        var xDelta = segment.End.X - segment.Start.X;
        var yDelta = segment.End.Y - segment.Start.Y;

        var u = ((X - segment.Start.X) * xDelta + (Y - segment.Start.Y) * yDelta) / (xDelta * xDelta + yDelta * yDelta);

        if (u < 0) {
            intersection = segment.Start;
        } else if (u > 1) {
            intersection = segment.End;
        } else {
            intersection = new Point(segment.Start.X + u * xDelta, segment.Start.Y + u * yDelta);
        }

        return Point.DistanceBetween(this, intersection);
    }
}