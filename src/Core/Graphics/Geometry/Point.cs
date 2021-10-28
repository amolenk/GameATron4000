namespace Amolenk.GameATron4000.Graphics.Geometry;

public record Point(double X, double Y)
{
    public static double DistanceBetween(Point p1, Point p2)
    {
        var distX = p2.X - p1.X;
        var distY = p2.Y - p1.Y;

        return Math.Sqrt(distX * distX + distY * distY);
    }
}