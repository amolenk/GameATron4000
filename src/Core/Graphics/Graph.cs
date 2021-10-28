// TODO Obsolete
// namespace Amolenk.GameATron4000.Graphics;

// public class Graph
// {
//     private Dictionary<Point, List<Point>> _adjacentList;

//     public IEnumerable<Point> Vertices => _adjacentList.Keys;

//     public IEnumerable<Line> Edges => _adjacentList.SelectMany(
//         _adjacent => _adjacent.Value.Select(
//             target => new Line(_adjacent.Key, target)));

//     public Graph()
//     {
//         _adjacentList = new();
//     }

//     public void AddVertex(Point vertex)
//     {
//         _adjacentList.TryAdd(vertex, new List<Point>());
//     }

//     public void AddEdge(Point source, Point target)
//     {
//         if (!_adjacentList.ContainsKey(source))
//         {
//             AddVertex(source);
//         }

//         var targets = _adjacentList[source];
//         if (!targets.Contains(target))
//         {
//             targets.Add(target);
//         }
//     }

//     public IEnumerable<Line> AStar(
//         Point source,
//         Point target,
//         Func<Point, Point, double> costHeuristic)
//     {
//         // The set of nodes already evaluated
//         List<Point> closedSet = new();
    
//         // The set of currently discovered nodes that are not evaluated yet.
//         // Initially, only the source node is known.
//         List<Point> openSet = new() { source };
    
//         // For each node, which node it can most efficiently be reached from.
//         // If a node can be reached from many nodes, cameFrom will eventually
//         // contain the most efficient previous step.
//         Dictionary<Point, Point> cameFrom = new();
    
//         // For each node, the cost of getting from the source node to that node.
//         Dictionary<Point, double> gScores = new();
    
//         // The cost of going from source to source is zero.
//         gScores.Add(source, 0);
    
//         // For each node, the total cost of getting from the source node to the
//         // target by passing by that node. That value is partly known, partly
//         // heuristic.
//         Dictionary<Point, double> fScores = new();
    
//         // For the first node, that value is completely heuristic.
//         fScores.Add(source, costHeuristic(source, target));

//         while (openSet.Count > 0)
//         {
//             var current = FindNodeWithLowestFScore(openSet, fScores);
//             if (current == target)
//             {
//                 return ReconstructPath(cameFrom, current);
//             }

//             // Move current from open to closed set.
//             openSet.Remove(current);
//             closedSet.Add(current);

//             foreach (var neighbour in _adjacentList[current])
//             {
//                 // Ignore the neighbour if it's already evaluated.
//                 if (closedSet.Contains(neighbour)) continue;
            
//                 // The distance from start to a neighbour.
//                 var tentativeGScore = gScores.ContainsKey(current)
//                     ? gScores[current] : 0;
//                 tentativeGScore += Point.DistanceBetween(current, neighbour);

//                 // Discovered a new node.
//                 if (!openSet.Contains(neighbour))
//                 { 
//                     openSet.Add(neighbour);
//                 }
//                 else if (tentativeGScore >= gScores[neighbour])
//                 {
//                     continue;
//                 }

//                 // This path is the best until now. Record it!
//                 cameFrom[neighbour] = current;
//                 gScores[neighbour] = tentativeGScore;
//                 fScores[neighbour] = gScores[neighbour] +
//                     costHeuristic(neighbour, target);
//             }
//         }

//         return Enumerable.Empty<Line>();
//     }

//     public void Draw(IGraphics graphics) =>
//         graphics.DrawLines(Edges, 2, 0x0000FF);

//     private Point FindNodeWithLowestFScore(
//         List<Point> openSet,
//         Dictionary<Point, double> fScores) =>
//             openSet.MinBy((point) => fScores.ContainsKey(point)
//                 ? fScores[point]
//                 : double.MaxValue)!;

//     private IEnumerable<Line> ReconstructPath(
//         Dictionary<Point, Point> cameFrom,
//         Point current)
//     {
//         List<Line> path = new();

//         while (cameFrom.ContainsKey(current)) {
//             var origin = cameFrom[current];
            
//             path.Insert(0, new Line(origin, current));

//             current = origin;
//         }

//         return path;
//     }
// }