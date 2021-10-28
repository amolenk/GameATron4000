namespace Amolenk.GameATron4000.Engine.Graphics.Geometry;

// https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
public static class Extensions
{
    private const double Epsilon = 1e-10;

    public static bool IsZero(this double d)
    {
        return Math.Abs(d) < Epsilon;
    }
}
