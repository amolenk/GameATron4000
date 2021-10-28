namespace Amolenk.GameATron4000.Graphics.Geometry;

public record Point(double X, double Y)
{
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


    // Adapted from https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
    // public double DistanceFromLine(Line line)
    // {
    //     var a = X - line.Start.X;
    //     var b = Y - line.Start.Y;
    //     var c = line.End.X - line.Start.X;
    //     var d = line.End.Y - line.Start.Y;
        
    //     var dot = a * c + b * d;
    //     var len_sq = c * c + d * d;
    //     var param = (len_sq != 0) ? dot / len_sq : -1;
        
    //     double xx, yy;
        
    //     if (param < 0) {
    //         xx = line.Start.X;
    //         yy = line.Start.Y;
    //     }
    //     else if (param > 1) {
    //         xx = line.End.X;
    //         yy = line.End.Y;
    //     }
    //     else {
    //         xx = line.Start.X + param * c;
    //         yy = line.Start.Y + param * d;
    //     }

    //     var dx = X - xx;
    //     var dy = Y - yy;

    //     return Math.Sqrt(dx * dx + dy * dy);
    // }
}