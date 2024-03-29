﻿namespace Amolenk.GameATron4000.Model;

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

    public IEnumerable<Point> FindShortestPath(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas)
    {
        // Make sure walkTo is inside walkbox.
        walkTo = SnapToWalkbox(walkTo, excludedAreas);

        var graph = CreateWalkGraph(walkFrom, walkTo, excludedAreas);

        return ComputeShortestPath(walkFrom, walkTo, graph)
            .Select(edge => edge.Target);
    }

    public Point SnapToWalkbox(
        Point point,
        IEnumerable<Polygon> excludedAreas = null!)
    {
        excludedAreas ??= Enumerable.Empty<Polygon>();

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

    public void Draw(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas,
        IGraphics graphics)
    {
        // Make sure walkTo is inside walkbox.
        walkTo = SnapToWalkbox(walkTo, excludedAreas);

        // Draw walkbox polygons
        graphics.DrawLines(
            _area.Edges,
            2,
            0xFFFFFF);

        // Draw excluded areas
        graphics.DrawLines(
            excludedAreas.SelectMany(area => area.Edges),
            2,
            0xFF0000);

        // Draw walk graph
        var graph = CreateWalkGraph(walkFrom, walkTo, excludedAreas);
        graphics.DrawLines(
            graph.Edges.Select(edge => new Line(edge.Source, edge.Target)),
            2,
            0x0000FF);
        
        // Find the shortest path and draw it with a green line.
        var path = ComputeShortestPath(walkFrom, walkTo, graph);
        graphics.DrawLines(
            path.Select(edge => new Line(edge.Source, edge.Target)),
            2,
            0x00FF00);
    }

    private AdjacencyGraph<Point, Edge<Point>> CreateWalkGraph(
        Point walkFrom,
        Point walkTo,
        IEnumerable<Polygon> excludedAreas)
    {
        var graph = new AdjacencyGraph<Point, Edge<Point>>();

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

    private IEnumerable<Edge<Point>> ComputeShortestPath(
        Point from,
        Point to,
        AdjacencyGraph<Point, Edge<Point>> graph)
    {
        var tryGetPaths = graph.ShortestPathsDijkstra(
            (edge) => Point.DistanceBetween(edge.Source, edge.Target),
            from);

        if (tryGetPaths(to, out IEnumerable<Edge<Point>> path))
        {
            return path;
        }

        return Enumerable.Empty<Edge<Point>>();
    }

    private void AddLineOfSightEdges(
        Point source,
        AdjacencyGraph<Point, Edge<Point>> graph,
        IEnumerable<Polygon> excludedAreas)
    {
        foreach (var target in graph.Vertices)
        {
            if (source != target
                && InLineOfSight(source, target, excludedAreas))
            {
                graph.AddEdge(new Edge<Point>(source, target));
            }
        }
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
                // In some cases a 'snapped' endpoint is just a little over the
                // line due to rounding errors.
                // So a 0.5 margin is used to tackle those cases. 
                if (source.DistanceToSegment(edge) > 0.5
                    && target.DistanceToSegment(edge) > 0.5)
                {
                    return false;
                }
            }
        }

        // Check that the midpoint is inside the walkbox.
        var midpoint = sightLine.GetMidpoint();
        if (!InArea(midpoint, _area, true))
        {
            return false;
        }

        // If there are any excluded areas in the walkboX, check that the
        // midpoint isn't in one of them.
        if (excludedAreas.Any(
            excludedArea => InArea(midpoint, excludedArea, false)))
        {
            return false;
        }

        return true;
    }

    private bool InArea(Point point, Polygon area, bool defaultTo)
    {
        // If we only use polygon.contains, we may get wrong results due to
        // rounding errors.
        foreach (var edge in area.Edges)
        {
            if (point.DistanceToSegment(edge) < 0.5)
            {
                return defaultTo;
            }
        }

        return area.Contains(point);
    }
}
