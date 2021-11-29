namespace Amolenk.GameATron4000.Graphics.Geometry;

public record Line(Point Start, Point End)
{
    public Point GetMidpoint() =>
        new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2);

    // Adapted from https://github.com/photonstorm/phaser/blob/v2.6.2/src/geom/Line.js
    public bool Intersects(
        Line other,
        out Point intersection,
        bool asSegment = true)
    {
        intersection = new Point(double.NaN, double.NaN);

        var a1 = End.Y - Start.Y;
        var a2 = other.End.Y - other.Start.Y;
        var b1 = Start.X - End.X;
        var b2 = other.Start.X - other.End.X;
        var c1 = (End.X * Start.Y) - (Start.X * End.Y);
        var c2 = (other.End.X * other.Start.Y) - (other.Start.X * other.End.Y);
        var denom = (a1 * b2) - (a2 * b1);

        if (denom == 0)
        {
            return false;
        }

        intersection = new Point(
            ((b1 * c2) - (b2 * c1)) / denom,
            ((a2 * c1) - (a1 * c2)) / denom);

        if (asSegment)
        {
            var uc = ((other.End.Y - other.Start.Y) * (End.X - Start.X) - (other.End.X - other.Start.X) * (End.Y - Start.Y));
            var ua = (((other.End.X - other.Start.X) * (Start.Y - other.Start.Y)) - (other.End.Y - other.Start.Y) * (Start.X - other.Start.X)) / uc;
            var ub = (((End.X - Start.X) * (Start.Y - other.Start.Y)) - ((End.Y - Start.Y) * (Start.X - other.Start.X))) / uc;

            return (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1);
        }

        return true;
    }
}
