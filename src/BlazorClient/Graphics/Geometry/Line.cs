namespace Amolenk.GameATron4000.Engine.Graphics.Geometry;

public record Line(Point Start, Point End)
{
    public Point GetMidpoint() =>
        new Point((Start.X + End.X) / 2, (Start.Y + End.Y) / 2);

    // Define Infinite (Using INT_MAX
    // caused overflow problems)
    static int INF = 10000;
  
    // Given three collinear points p, q, r,
    // the function checks if point q lies
    // on line segment 'pr'
    static bool onSegment(Point p, Point q, Point r)
    {
        if (q.X <= Math.Max(p.X, r.X) &&
            q.X >= Math.Min(p.X, r.X) &&
            q.Y <= Math.Max(p.Y, r.Y) &&
            q.Y >= Math.Min(p.Y, r.Y))
        {
            return true;
        }
        return false;
    }
 
    // To find orientation of ordered triplet (p, q, r).
    // The function returns following values
    // 0 --> p, q and r are collinear
    // 1 --> Clockwise
    // 2 --> Counterclockwise
    static int orientation(Point p, Point q, Point r)
    {
        var val = (q.Y - p.Y) * (r.X - q.X) -
                (q.X - p.X) * (r.Y - q.Y);
 
        if (val == 0)
        {
            return 0; // collinear
        }
        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }

    // Based on https://github.com/photonstorm/phaser/blob/v2.6.2/src/geom/Line.js
    public bool Intersects2(Line other, out Point intersection, bool asSegment = true)
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

    // https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
    public bool Intersects(
        Line other,
        out Vector intersection,
        bool considerCollinearOverlapAsIntersect = false)
    {
        var p = new Vector(Start.X, Start.Y);
        var p2 = new Vector(End.X, End.Y);
        var q = new Vector(other.Start.X, other.Start.Y);
        var q2 = new Vector(other.End.X, other.End.Y);

        intersection = new Vector();

        var r = p2 - p;
        var s = q2 - q;
        var rxs = r.Cross(s);
        var qpxr = (q - p).Cross(r);

        // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
        if (rxs.IsZero() && qpxr.IsZero())
        {
            // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
            // then the two lines are overlapping,
            if (considerCollinearOverlapAsIntersect)
                if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                    return true;

            // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
            // then the two lines are collinear but disjoint.
            // No need to implement this expression, as it follows from the expression abovother.Start.
            return false;
        }

        // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
        if (rxs.IsZero() && !qpxr.IsZero())
            return false;

        // t = (q - p) x s / (r x s)
        var t = (q - p).Cross(s) / rxs;

        // u = (q - p) x r / (r x s)

        var u = (q - p).Cross(r) / rxs;

        // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
        // the two line segments meet at the point p + t r = q + u s.
        if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
        {
            // We can calculate the intersection point using either t or u.
            intersection = p + t * r;

            // Make sure we have a proper intersection.
            if (!intersection.Equals(q)
                && !intersection.Equals(q2))
            {
                return true;
            }
        }

        // 5. Otherwise, the two line segments are not parallel but do not intersect.
        return false;
    }
}
