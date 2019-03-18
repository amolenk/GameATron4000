/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Graph } from "./graph";

// All polygons must be defined clock-wise for this to work properly.
// (The algorithm for finding concave vertices is dependent on this).
export class Walkbox {

    private WORLD_HEIGHT = 450;

    private _polygons: Array<Phaser.Polygon>;
    private _baseGraph: Graph;

    constructor(polygons: Phaser.Polygon[]) {
        this._polygons = polygons;
        this._baseGraph = this.createBaseGraph();
    }

    public snapTo(x: number, y: number): Phaser.Point {

        if (this._polygons[0].contains(x, y)) {
            if (this._polygons.length > 1) {
                for (let i = 1; i < this._polygons.length; i++) {
                    if (this._polygons[i].contains(x, y)) {
                        return this.getIntersection(this._polygons[i], x, y);
                    }
                }
            }
            return new Phaser.Point(x, y);
        }

        return this.getIntersection(this._polygons[0], x, y);
    }

    // TODO return type
    public findPath(fromX: number, fromY: number, toX: number, toY: number): Phaser.Point[] {

        // Create the complete walk graph including the 'from' and 'to' points.
        const from = new Phaser.Point(fromX, fromY);
        const to = this.snapTo(toX, toY);
        const walkGraph = this.createWalkGraph(from, to, this._baseGraph);

        return walkGraph.aStar(from, to);
    }

    public debug(fromX: number, fromY: number, toX: number, toY: number, debug: Phaser.Utils.Debug) {

        const from = new Phaser.Point(fromX, fromY);
        const to = this.snapTo(toX, toY);
        const walkGraph = this.createWalkGraph(from, to, this._baseGraph);

        // Draw the walk box.
        for (let polygon of this._polygons) {
            for (let i = 0; i < polygon.points.length; i++) {
                var edgeStart = <Phaser.Point>polygon.points[i];
                var edgeEnd = <Phaser.Point>polygon.points[(i + 1) % polygon.points.length];
                debug.geom(new Phaser.Line(edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y), 'rgb(0,0,255)');
            }
        }

        // Draw the walk graph over it.
        walkGraph.debug(debug);

        // If the 'from' and 'to' positions are in line of sight, draw a green
        // line between them. No need to do any path finding.
        if (this.inLineOfSight(from, to)) {
            debug.geom(new Phaser.Line(fromX, fromY, to.x, to.y), 'rgb(0,255,0)');
        } else { // Otherwise, draw a red line and do some path finding.
            debug.geom(new Phaser.Line(fromX, fromY, to.x, to.y), 'rgb(255,0,0)');

            const path = walkGraph.aStar(from, to);
            if (path) {
                for (let i = 0; i < path.length - 1; i++) {
                    debug.geom(new Phaser.Line(path[i].x, path[i].y, path[i+1].x, path[i+1].y), 'rgb(0,255,0)');
                }
            }
        }

        // Draw the 'from' and 'to' points.
        debug.geom(new Phaser.Circle(fromX, fromY, 10), 'rgb(255,255,0)', true, 0);
        debug.geom(new Phaser.Circle(to.x, to.y, 10), 'rgb(255,0,0)', true, 0);
    }

    private createWalkGraph(from: Phaser.Point, to: Phaser.Point, baseGraph: Graph): Graph {
        // Create a new graph and add nodes for the 'from' and 'to' points
        // (including line of sight edges).
        const result = baseGraph.clone();

        result.addVertex(from);
        result.addVertex(to);

        this.addLineOfSightEdges(from, result);
        this.addLineOfSightEdges(to, result);

        return result;
    }

    private createBaseGraph(): Graph {
        let first = true;
        const result = new Graph();

        for (let polygon of this._polygons) {
            for (let i = 0; i < polygon.points.length; i++) {

                // Include all concave vertices for the first (outer) polygon,
                // and all convex vertices for the other (inner) polygons.
                if (this.isVertexConcave(<Phaser.Point[]>polygon.points, i) == first) {
                    const point = <Phaser.Point>polygon.points[i];
                    result.addVertex(point);
                }
            }
            first = false;
        }

        // Connect all the vertices which are within line of sight.
        const vertices = result.vertices;
        for (var i = 0; i < vertices.length; i++) {
            this.addLineOfSightEdges(vertices[i], result);
        }

        return result;
    }

    private isVertexConcave(vertices: Phaser.Point[], index: number) : boolean {
        var current = vertices[index];
        var next = vertices[(index + 1) % vertices.length];
        var previous = vertices[index == 0 ? vertices.length - 1 : index - 1];
        var deltaLeft = new Phaser.Point(current.x - previous.x, current.y - previous.y);
        var deltaRight = new Phaser.Point(next.x - current.x, next.y - current.y);
        var cross = (deltaLeft.x * deltaRight.y) - (deltaLeft.y * deltaRight.x);
        return cross < 0;
    }

    // TODO Voegen we niet teveel Edges toe????
    private addLineOfSightEdges(from: Phaser.Point, graph: Graph) {
        const vertices = graph.vertices;

        // Connect all the vertices which are within line of sight.
        for (var i = 0; i < vertices.length; i++) {
            if (this.inLineOfSight(from, vertices[i])) {
                graph.addEdge(from, vertices[i]);
            }
        }
    }
    
    private inLineOfSight(start: Phaser.Point, end: Phaser.Point) : boolean {

        // Not in line of sight if any edge is intersected by the start-end line.
        // TODO Check other (inner) polygons as well.
        for (let polygon of this._polygons) {
            for (var i = 0; i < polygon.points.length; i++) {
                var edgeStart = <Phaser.Point>polygon.points[i];
                var edgeEnd = <Phaser.Point>polygon.points[(i + 1) % polygon.points.length];
                var intersection = Phaser.Line.intersectsPoints(start, end, edgeStart, edgeEnd, true);
                if (intersection) {

                    // In some cases a 'snapped' endpoint is just a little over the line due to rounding errors.
                    // So a 0.5 margin is used to tackle those cases. 
                    if (this.distanceBetweenPointAndLine(start.x, start.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) > 0.5
                        && this.distanceBetweenPointAndLine(end.x, end.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) > 0.5) {
                        return false;
                    }
                }
            }
        }

        // If there are any holes in the walkbox, check that the midpoint is not in a hole.
        if (this._polygons.length > 1) {
            const line = new Phaser.Line(start.x, start.y, end.x, end.y);
            const midPoint = line.midPoint();
            for (let i = 1; i < this._polygons.length; i++) {
                if (this.inPolygon(midPoint, this._polygons[i])) {
                    return false;
                }
            }
        }

        return true;
    }

    private inPolygon(point: Phaser.Point, polygon: Phaser.Polygon) : boolean {

        if (polygon.contains(point.x, point.y)) {

            // We may be getting a false positives due to rounding errors.
            for (var i = 0; i < polygon.points.length; i++) {
                var edgeStart = <Phaser.Point>polygon.points[i];
                var edgeEnd = <Phaser.Point>polygon.points[(i + 1) % polygon.points.length];

                if (this.distanceBetweenPointAndLine(point.x, point.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) < 0.5) {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    //https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
    private distanceBetweenPointAndLine(x: number, y: number, x1: number, y1: number, x2: number, y2: number) {

        var A = x - x1;
        var B = y - y1;
        var C = x2 - x1;
        var D = y2 - y1;
        
        var dot = A * C + B * D;
        var len_sq = C * C + D * D;
        var param = -1;
        if (len_sq != 0) //in case of 0 length line
            param = dot / len_sq;
        
        var xx, yy;
        
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

        return Math.sqrt(dx * dx + dy * dy);
    }

    private getIntersection(polygon: Phaser.Polygon, x: number, y: number): Phaser.Point {

        let result: Phaser.Point;
        let minDistance: number;

        for (let i = 0; i < polygon.points.length; i++) {

            const intersection = Phaser.Line.intersectsPoints(
                new Phaser.Point(x, 0),
                new Phaser.Point(x, this.WORLD_HEIGHT),
                <Phaser.Point>polygon.points[i],
                <Phaser.Point>polygon.points[(i + 1) % polygon.points.length]);

            if (intersection) {
                const distance = Math.abs(y - intersection.y);
                if (minDistance == null || distance < minDistance) {
                    minDistance = distance;
                    result = intersection;
                }
            }
        }

        return result;
    }
}