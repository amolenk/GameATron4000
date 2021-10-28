namespace Amolenk.GameATron4000.Engine.Graphics.Geometry;

// https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
public class Vector
{
    public double X { get; }

    public double Y { get; }

    public Vector() : this(double.NaN, double.NaN)
    {
    }

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector operator -(Vector v, Vector w) =>
        new Vector(v.X - w.X, v.Y - w.Y);

    public static Vector operator +(Vector v, Vector w) =>
        new Vector(v.X + w.X, v.Y + w.Y);

    public static double operator *(Vector v, Vector w) =>
        v.X * w.X + v.Y * w.Y;

    public static Vector operator *(Vector v, double mult) =>
        new Vector(v.X * mult, v.Y * mult);

    public static Vector operator *(double mult, Vector v) =>
        new Vector(v.X * mult, v.Y * mult);

    public double Cross(Vector v) =>
        X * v.Y - Y * v.X;

    public override bool Equals(object? obj)
    {
        var v = obj as Vector;
        if (v != null)
        {
            return (X - v.X).IsZero() && (Y - v.Y).IsZero();
        }
        return false;
    }

    public override int GetHashCode() =>
        HashCode.Combine(X.IsZero() ? 0 : X, Y.IsZero() ? 0 : Y);
}
