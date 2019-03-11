/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

export class Graph {

    private _adjacentList: Map<Phaser.Point, Array<Phaser.Point>>;

    constructor() {
        this._adjacentList = new Map<Phaser.Point, Array<Phaser.Point>>(); 
    }

    public addVertex(vertex: Phaser.Point) { 
        this._adjacentList.set(vertex, []); 
    }

    public addEdge(vertex1: Phaser.Point, vertex2: Phaser.Point) {
        // Undirected graph, so create edges both ways.
        this._adjacentList.get(vertex1).push(vertex2);
        this._adjacentList.get(vertex2).push(vertex1);
    } 

    public reconstructPath(cameFrom: Map<Phaser.Point, Phaser.Point>, current: Phaser.Point): Phaser.Point[] {
        const result = [];
        result.push(current);

        while (cameFrom.has(current)) {
            current = cameFrom.get(current);
            result.push(current);
        }

        return result;
    }

    public aStar(start: Phaser.Point, goal: Phaser.Point) {

        // TODO Based on Wikipedia pseudocode

        // The set of nodes already evaluated
        const closedSet: Phaser.Point[] = [];
    
        // The set of currently discovered nodes that are not evaluated yet.
        // Initially, only the start node is known.
        const openSet = [];
        openSet.push(start);
    
        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        const cameFrom = new Map<Phaser.Point, Phaser.Point>();
    
        // For each node, the cost of getting from the start node to that node.
        const gScore = new Map<Phaser.Point, number>();// map with default value of Infinity
    
        // The cost of going from start to start is zero.
        gScore.set(start, 0);
    
        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        const fScore = new Map<Phaser.Point, number>(); // default of infinity/
    
        // For the first node, that value is completely heuristic.
        fScore.set(start, this.heuristicCostEstimate(start, goal));
    
        while (openSet.length > 0) {
            let current = this.findNodeWithLowestFScore(openSet, fScore);
            if (current == goal) {
                return this.reconstructPath(cameFrom, current)
            }

            // Remove current
            openSet.splice(openSet.indexOf(current), 1);
            closedSet.push(current);

            // console.log(current);

            for (let neighbour of this._adjacentList.get(current)) {

                // console.log(neighbour);

                if (closedSet.indexOf(neighbour) != -1) {
                    continue; // Ignore the neighbour which is already evaluated.
                }

                // The distance from start to a neighbour.
                const tentativeGScore = (gScore.has(current) ? gScore.get(current) : 0)
                    + Phaser.Math.distance(current.x, current.y, neighbour.x, neighbour.y);

                if (openSet.indexOf(neighbour) == -1) { // Discovered a new node.
                    openSet.push(neighbour);
                } else if (tentativeGScore >= gScore.get(neighbour)) {
                    continue;
                }

                // This path is the best until now. Record it!
                cameFrom.set(neighbour, current);
                gScore.set(neighbour, tentativeGScore);
                fScore.set(neighbour, gScore.get(neighbour) + this.heuristicCostEstimate(neighbour, goal));

                // console.log('gScore: ' + tentativeGScore);
                // console.log('fScore: ' + fScore.get(neighbour));
            }
        }

        // while openSet is not empty
        //     current := the node in openSet having the lowest fScore[] value
        //     if current = goal
        //         return reconstruct_path(cameFrom, current)
    
        //     openSet.Remove(current)
        //     closedSet.Add(current)
    
        //     for each neighbor of current
        //         if neighbor in closedSet
        //             continue		// Ignore the neighbor which is already evaluated.
    
        //         // The distance from start to a neighbor
        //         tentative_gScore := gScore[current] + dist_between(current, neighbor)
    
        //         if neighbor not in openSet	// Discover a new node
        //             openSet.Add(neighbor)
        //         else if tentative_gScore >= gScore[neighbor]
        //             continue
    
        //         // This path is the best until now. Record it!
        //         cameFrom[neighbor] := current
        //         gScore[neighbor] := tentative_gScore
        //         fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)
    }

    private findNodeWithLowestFScore(openSet: Phaser.Point[], fScore: Map<Phaser.Point, number>): Phaser.Point {
        let result: Phaser.Point;
        let minFScore: number = Number.MAX_SAFE_INTEGER;

        for (let node of openSet) {
            if (fScore.has(node)) {
                const nodeFScore = fScore.get(node);
                if (nodeFScore < minFScore) {
                    minFScore = nodeFScore;
                    result = node;
                }
            }
        }

        return result;
    }

    public debug(debug: Phaser.Utils.Debug) {

        for (let vertex of this._adjacentList) {
            const start = vertex[0];
            debug.geom(new Phaser.Circle(start.x, start.y, 5), 'rgb(0,0,255)', true, 0);

            for (let end of vertex[1]) {
                debug.geom(new Phaser.Line(start.x, start.y, end.x, end.y), 'rgb(255,255,255)');
            }
        }
    }

    private heuristicCostEstimate(start: Phaser.Point, goal: Phaser.Point) {
        // Simply use the exact distance between start and goal as the heuristic for the A* algorithm.
        return Phaser.Math.distance(start.x, start.y, goal.x, goal.y);
    }
}