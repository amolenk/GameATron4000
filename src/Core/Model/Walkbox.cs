namespace Amolenk.GameATron4000.Model;

public class Walkbox
{
    private readonly Polygon _area;

    public Walkbox(Polygon area)
    {
        if (area.Vertices.Count < 3)
        {
            throw new ArgumentOutOfRangeException(
                nameof(area),
                "Walkbox area must contain at least 3 vertices.");
        }

        _area = area;
    }

    public static Walkbox FromVertices(params Point[] vertices)
        => new Walkbox(new Polygon(vertices));

    public IEnumerable<Line> FindShortestPath(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas)
    {

        // Make sure walkTo is inside walkbox.
        walkTo = SnapToWalkbox(walkTo, excludedAreas);

        

        var graph = CreateWalkGraph(walkFrom, walkTo, excludedAreas);


        return ComputeShortestPath(walkFrom, walkTo, excludedAreas, graph);
    }

    public void Draw(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas,
        IGraphics graphics)
    {
        _log = true;

        // Make sure walkTo is inside walkbox.
        walkTo = SnapToWalkbox(walkTo, excludedAreas);

        // Draw walkbox polygons
        graphics.DrawLines(
            new[] { _area }
                .Concat(excludedAreas)
                .SelectMany(area => area.Edges),
            2,
            0xFFFFFF);

        var graph = CreateWalkGraph(walkFrom, walkTo, excludedAreas);
        graph.Draw(graphics);
        
        // Find the shortest path and draw it with a green line.
        var path = ComputeShortestPath(walkFrom, walkTo, excludedAreas, graph);
//        var lines = path.Zip(path.Skip(1), (p1, p2) => new Line(p1, p2));
        graphics.DrawLines(path, 2, 0x00FF00);

        // var p = new Point(560, 400);
        // var q = new Point(429, 370);

        // var isVisible = InLineOfSight(p, q, excludedAreas);

        // graphics.DrawLines(new []
        // {
        //     new Line(p, q)
        // }, 1, 0xFF0000);

        // Log("Result: " + isVisible);



        _log = false;
    }

    private bool _log = false;

    private void Log(string message)
    {
        if (_log) Console.WriteLine(message);
    }

    private Graph CreateWalkGraph(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas)
    {
        var graph = new Graph();

        // Include all concave vertices for the main area.
        // Concave polygons have curves inward, which means that it's not
        // possible to have a direct walk lines for all locations. We'll
        // have to route around these corners.
        for (var i = 0; i < _area.Vertices.Count; i++)
        {
            if (_area.IsConcaveAt(i))
            {
                graph.AddVertex(_area.Vertices[i]);
            }
        }

        // Include all convex vertices for the excluded areas.
        foreach (var excludedArea in excludedAreas)
        {
            for (var i = 0; i < excludedArea.Vertices.Count; i++)
            {
                if (!excludedArea.IsConcaveAt(i))
                {
                    graph.AddVertex(excludedArea.Vertices[i]);
                }
            }
        }

        // Add vertices for the from/to points.
        graph.AddVertex(walkFrom);
        graph.AddVertex(walkTo);

        // Connect all the vertices which are within line of sight
        // of each other.
        foreach (var vertex in graph.Vertices)
        {
            AddLineOfSightEdges(vertex, graph, excludedAreas);
        }

        return graph;
    }

    private IEnumerable<Line> ComputeShortestPath(
        Point from,
        Point to,
        IEnumerable<Polygon> excludedAreas,
        Graph graph)
    {
        // TODO Test with draw
        // if (InLineOfSight(from, to, excludedAreas))
        // {
        //     return new []
        //     {
        //         new Line(from, to)
        //     };
        // }
        // else
        // {
        //     //Console.WriteLine($"Not in line of sight: {from} {to}");
        // }


        return graph.AStar(from, to, (source, target) => Point.DistanceBetween(source, target));


    }

    private Point SnapToWalkbox(Point point, IEnumerable<Polygon> excludedAreas)
    {
        if (_area.Contains(point))
        {
            foreach (var excludedArea in excludedAreas)
            {
                if (excludedArea.Contains(point))
                {
                    return excludedArea.FindClosestPoint(point) ?? point;
                }
            }

            return point;
        }

        return _area.FindClosestPoint(point) ?? point;
    }

    private void AddLineOfSightEdges(
        Point source,
        Graph graph,
        IEnumerable<Polygon> excludedAreas)
    {
        foreach (var target in graph.Vertices)
        {
            if (source != target
                && InLineOfSight(source, target, excludedAreas))
            {
                graph.AddEdge(source, target);
            }
        }
    }

    //https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
    private double distanceBetweenPointAndLine(double x, double y, double x1, double y1, double x2, double y2) {

        var A = x - x1;
        var B = y - y1;
        var C = x2 - x1;
        var D = y2 - y1;
        
        var dot = A * C + B * D;
        var len_sq = C * C + D * D;
        double param = -1;
        if (len_sq != 0) //in case of 0 length line
            param = dot / len_sq;
        
        double xx, yy;
        
        if (param < 0) {
            xx = x1;
            yy = y1;
        }
        else if (param > 1) {
            xx = x2;
            yy = y2;
        }
        else {
            xx = x1 + param * C;
            yy = y1 + param * D;
        }

        var dx = x - xx;
        var dy = y - yy;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    private bool InLineOfSight(
        Point source,
        Point target,
        IEnumerable<Polygon> excludedAreas)
    {
        var sightLine = new Line(source, target);

        // Not in line of sight if the line is intersected by any edge.
        var edges = new[] { _area }
            .Concat(excludedAreas)
            .SelectMany(area => area.Edges);

        foreach (var edge in edges)
        {
            if (sightLine.Intersects(edge, out Point intersection))
            {
                // In some cases a 'snapped' endpoint is just a little over the line due to rounding errors.
                // So a 0.5 margin is used to tackle those cases. 
                if (this.distanceBetweenPointAndLine(source.X, source.Y, edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y ) > 0.5
                    && this.distanceBetweenPointAndLine(target.X, target.Y, edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y ) > 0.5) {
                    return false;
                }
            }
        }

        // Check that the midpoint is inside the walkbox.
        var midpoint = sightLine.GetMidpoint();
        if (!inPolygon(midpoint, _area, true))
        {
            return false;
        }

        // If there are any excluded areas in the walkboX, check that the
        // midpoint isn't in one of them.
        if (excludedAreas.Any(excludedArea => inPolygon(midpoint, excludedArea, false)))
        {
            return false;
        }

        return true;
    }

    private bool inPolygon(Point point, Polygon polygon, bool defaultTo)
    {
        // If we use polygon.contains, we may get wrong results due to rounding errors.
        foreach (var edge in polygon.Edges)
        {
            if (this.distanceBetweenPointAndLine(point.X, point.Y, edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y) < 0.5){
                return defaultTo;
            }
        }

        return polygon.Contains(point);
    }

    private void DrawWalkbox(
        IEnumerable<Polygon> excludedAreas,
        IGraphics graphics)
    {
        var edges = new[] { _area }
            .Concat(excludedAreas)
            .SelectMany(area => area.Edges);

        graphics.DrawLines(edges, 2, 0xFFFFFF);
    }
}
