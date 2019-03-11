/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Actor } from "./actor"
import { Layers } from "./layers"
import { Narrator } from "./narrator"
import { RoomObject } from "./room-object"
import { UIMediator } from "./ui-mediator"
import { Graph } from "./graph"

export class Room {

    public narrator: Narrator;

    private game: Phaser.Game;
    private roomObjects: Array<RoomObject>;
    private actionMap: Map<string, Function>;
    private uiMediator: UIMediator;
    private layers: Layers;
    private graphics: Phaser.Graphics;

    constructor(private name: string) {
        this.roomObjects = new Array<RoomObject>();
        this.actionMap = new Map<string, Function>();
    }

    public create(
        game: Phaser.Game,
        uiMediator: UIMediator,
        layers: Layers) {
        
        this.game = game;
        this.uiMediator = uiMediator;
        this.layers = layers;
        this.graphics = game.add.graphics(0, 0);

       var background = this.game.add.sprite(0, 0, "room-" + this.name);
       background.inputEnabled = true;
//        TODO Temp?
        // background.events.onInputDown.add((args: any, pointer: Phaser.Pointer) => {
        //     this.temp(pointer.positionDown);
        // });

        this.layers.background.add(background);

        this.narrator = new Narrator(game, layers);
        this.narrator.create();
    }

    // Clockwise, necessary for finding the concave vertices
    private walkbox = new Phaser.Polygon(
        new Phaser.Point(799, 305),
        new Phaser.Point(799, 318),
        new Phaser.Point(590, 364),
        new Phaser.Point(799, 416),
        new Phaser.Point(799, 449),
        new Phaser.Point(568, 449),
        new Phaser.Point(380, 375),
        new Phaser.Point(0, 324),
        new Phaser.Point(0, 316),
        new Phaser.Point(130, 320),
        new Phaser.Point(430, 350),
        new Phaser.Point(799, 305)
    );

    public update() {



    
        // var p = new Phaser.Point(740, 430);

        // this.graphics.clear();

        // this.graphics.lineStyle(3, 0x00ff00);
        // this.graphics.moveTo(10, 10);
        // this.graphics.lineTo(300, 300);

        // this.graphics.lineStyle(3, 0x0000ff);




        // mainwalkgraph = new Graph();
        // var first:Bool = true;
        // vertices_concave = new Array();
        // for (polygon in polygons) {
        //     if (polygon != null &amp;&amp; polygon.vertices != null &amp;&amp; polygon.vertices.length &gt; 2) {
        //         for (i in 0...polygon.vertices.length) {
        //             if (IsVertexConcave(polygon.vertices, i) == first) {
        //                 var index:Int = vertices_concave.length;
        //                 vertices_concave.push(polygon.vertices[i]);
        //                 mainwalkgraph.addNode(new GraphNode(new Vector(polygon.vertices[i].x, polygon.vertices[i].y)));
        //             }
        //         }
        //     }
        //     first = false;
        // }

        // this.graphics.drawPolygon(this.walkbox);
 
        // this.graphics.moveTo(p.x, p.y);
        // this.graphics.lineTo(this.game.input.x, this.game.input.y);

        // this.graphics.lineStyle(3, 0xff0000);
        // this.graphics.drawCircle(700, 328, 2);

        // this.graphics.lineStyle(3, 0x00ff00);
        // this.graphics.drawCircle(750, 430, 2);


        // walkGraph.draw(this.graphics);

        // this.graphics.endFill();
        

    }

    public debug() {

        const walkBoxPoints = <Phaser.Point[]>this.walkbox.points;

        let targetX = this.game.input.x;
        let targetY = this.game.input.y;

        // Snap to walkbox
        if (!this.walkbox.contains(targetX, targetY)) {
            const snappedPoint = this.pointToWalkBox(new Phaser.Point(this.game.input.x, this.game.input.y), walkBoxPoints);
            targetX = snappedPoint.x;
            targetY = snappedPoint.y;
        }

        const target = new Phaser.Point(targetX, targetY);

        // Create graph
        var first = true;
        var concaveVertices = [];
        var walkGraph = new Graph();

        var currentPosition = new Phaser.Point(700, 420);

        for (var i = 0; i < walkBoxPoints.length; i++) {

            if (this.IsVertexConcave(walkBoxPoints, i) == first) {
                concaveVertices.push(walkBoxPoints[i]);

                // Add vertex to graph.
                walkGraph.addVertex(walkBoxPoints[i]);
            }
        }

        // Add source and target.
        walkGraph.addVertex(currentPosition);
        concaveVertices.push(currentPosition);
        walkGraph.addVertex(target);
        concaveVertices.push(target);

        // Connect all the vertices which are within line of sight.
        for (var i = 0; i < concaveVertices.length; i++) {
            for (var j = 0; j < concaveVertices.length; j++) {
                const first = concaveVertices[i];
                const second = concaveVertices[j];
                if (this.InLineOfSight(first, second)) {
                    walkGraph.addEdge(first, second);
                }
            }
        }


        // Draw stuff

        this.game.debug.geom(new Phaser.Circle(currentPosition.x, currentPosition.y, 10), 'rgb(255,255,0)', true, 0);

        for (var i = 0; i < walkBoxPoints.length; i++) {
            var edgeStart = walkBoxPoints[i];
            var edgeEnd = walkBoxPoints[(i + 1) % walkBoxPoints.length];
            this.game.debug.geom(new Phaser.Line(edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y), 'rgb(0,0,255)');
        }

        // for (const vertex of concaveVertices) {
        //     this.game.debug.geom(new Phaser.Circle(vertex.x, vertex.y, 5), 'rgb(255,255,255)', true, 0);
        // }

        walkGraph.debug(this.game.debug);
        
        this.game.debug.geom(new Phaser.Circle(targetX, targetY, 10), 'rgb(255,0,0)', true, 0);

        if (this.InLineOfSight(currentPosition, new Phaser.Point(targetX, targetY))) {
            this.game.debug.geom(new Phaser.Line(currentPosition.x, currentPosition.y, targetX, targetY), 'rgb(0,255,0)');
        } else {
            this.game.debug.geom(new Phaser.Line(currentPosition.x, currentPosition.y, targetX, targetY), 'rgb(255,0,0)');
        }

        // Real pathfinding!
       const path = walkGraph.aStar(currentPosition, target);
        for (let i = 0; i < path.length - 1; i++) {
            this.game.debug.geom(new Phaser.Line(path[i].x, path[i].y, path[i+1].x, path[i+1].y), 'rgb(0,255,0)');
        }
    }

    private pointToWalkBox(point: Phaser.Point, walkBoxPoints: Phaser.Point[]) {
        let result: Phaser.Point;
        let minDistance: number;

        for (let i = 0; i < walkBoxPoints.length; i++) {

            const intersection = Phaser.Line.intersectsPoints(
                new Phaser.Point(point.x, 0),
                new Phaser.Point(point.x, 450),
                walkBoxPoints[i],
                walkBoxPoints[(i + 1) % walkBoxPoints.length]);

            if (intersection) {
                const distance = Math.abs(point.y - intersection.y);
                if (minDistance == null || distance < minDistance) {
                    minDistance = distance;
                    result = intersection;
                }
            }
        }

        return result;
    }

    private IsVertexConcave(vertices: Phaser.Point[], index: number) : boolean {
        var current = vertices[index];
        var next = vertices[(index + 1) % vertices.length];
        var previous = vertices[index == 0 ? vertices.length - 1 : index - 1];
        var deltaLeft = new Phaser.Point(current.x - previous.x, current.y - previous.y);
        var deltaRight = new Phaser.Point(next.x - current.x, next.y - current.y);
        var cross = (deltaLeft.x * deltaRight.y) - (deltaLeft.y * deltaRight.x);
        return cross < 0;
    }

    private InLineOfSight(start: Phaser.Point, end: Phaser.Point) : boolean {

        var epsilon = 0.5;

        // Check if the points are contained within the walkbox polygon.
        // if (!this.walkbox.contains(start.x, start.y)
        //     || !this.walkbox.contains(end.x, end.y)) {
        //     return false;
        // }

        // If the start and end location are basically the same, they're in line of sight.
        // if (Point.subtract(start, end).getMagnitude() < epsilon) {
        //     return true;
        // }

        // Not in line of sight in any edge is intersected by the start-end line.
        const walkBoxPoints = <Phaser.Point[]>this.walkbox.points;

        for (var i = 0; i < walkBoxPoints.length; i++) {
            var edgeStart = walkBoxPoints[i];
            var edgeEnd = walkBoxPoints[(i + 1) % walkBoxPoints.length];
            var intersection = Phaser.Line.intersectsPoints(start, end, edgeStart, edgeEnd, true);
            if (intersection) {

                // In some cases a 'snapped' endpoint is just a little over the line due to rounding errors.
                // So a 0.5 margin is used to tackle those cases. 
                if (//this.pDistance(start.x, start.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) > 0.5
                    this.pDistance(start.x, start.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) > 0.5
                    && this.pDistance(end.x, end.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ) > 0.5) {
                    // console.log(
                    //     start.x + ',' + start.y + ' -> ' + end.x + ',' + end.y + ' / ' + edgeStart.x + ',' + edgeStart.y + ' -> ' + edgeEnd.x + ',' + edgeEnd.y + ' : ' +
                    //     this.pDistance(end.x, end.y, edgeStart.x, edgeStart.y, edgeEnd.x, edgeEnd.y ));
                    return false;
                }
            }
        }

        return true;
    }

    //https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
    private pDistance(x: number, y: number, x1: number, y1: number, x2: number, y2: number) {

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

    public LineSegmentsCross(start1 : Phaser.Point, end1 : Phaser.Point, start2 : Phaser.Point, end2 : Phaser.Point) : boolean
    {
        const denominator = ((end1.x - start1.x) * (end2.y - start2.y)) - ((end1.y - start1.y) * (end2.x - start2.x));
        
        if (denominator == 0) {
            return false;
        }

        const numerator1 = ((start1.y - start2.y) * (end2.x - start2.x)) - ((start1.x - start2.x) * (end2.y - start2.y));
        const numerator2 = ((start1.y - start2.y) * (end1.x - start1.x)) - ((start1.x - start2.x) * (end1.y - start1.y));

        if (numerator1 == 0 || numerator2 == 0) {
            return false;
        }

        const r = numerator1 / denominator;
        const s = numerator2 / denominator;

        return (r > 0 && r < 1) && (s > 0 && s < 1);
    }

    public temp(destination: Phaser.Point) {
        // Load the walk map into memory
        var bmd = this.game.make.bitmapData(800, 450);
        bmd.load('map');

        var walkableGrid = [];
        
        var graphics = this.game.add.graphics(0, 0);
        graphics.beginFill(0x0000FF, 1);

        for (var y = 0; y < 450; y++) {
            var row : any[] = walkableGrid[y] = [];
        
            for (var x = 0; x < 800; x++) {
            row[x] = (bmd.getPixelRGB(x, y).g == 64) ? 0 : 1;//.rgba === walkableRGB) ? 0 : 1;

        //     if (x % 10 == 0 && y % 10 == 0 && row[x] == 0)
        //         graphics.drawCircle(x, y, 10);
            }
        }

        //var actor = new Actor("actor-guy", "guy", "White", "Front");
        //this.room.addActor(actor, 640, 430);

        // var easystar = new js();
        // easystar.setGrid(walkableGrid);
        // easystar.setAcceptableTiles([0]);
        // easystar.enableDiagonals();

        // var self = this;

        // console.log(destination);

        // easystar.findPath(750, 430, destination.x, destination.y, (path) => {
        //     if (path === null) {
        //         console.log("Path was not found.");
        //     } else {
        //         console.log("Path was found. The first Point is " + path[0].x + " " + path[0].y);

        //         var parts = this.fixPath(path);

        //         graphics.beginFill(0xFF0000, 1);

        //         for (var part of parts) {
        //             console.log(part);
        //             graphics.drawCircle(part.x, part.y, 5);
        //     }

//                actor.moveTo(path);
                // var graphics = self.game.add.graphics(0, 0);

                // for (var spot of path) {
                //     graphics.drawCircle(spot.x, spot.y, 5);
                // }
        //     }
        // })

        // setInterval(() => easystar.calculate());
        
    }

    // public fixPath(path: any) {
    //     var parts = [];
    //     var dx = path[0].x - path[1].x;
    //     var dy = path[0].y - path[1].y;
    //     var i = 1;
    //     while (i < path.length - 1) {

    //         var ddx = path[i].x - path[i + 1].x;
    //         var ddy = path[i].y - path[i + 1].y;

    //         if (ddx != dx || ddy != dy) {

    //             parts.push({
    //                 x: path[i].x,
    //                 y: path[i].y
    //             });

    //             dx = ddx;
    //             dy = ddy;
    //         }
    //         i += 1;
    //     }
    //     parts.push({
    //         x: path[i].x,
    //         y: path[i].y
    //     });
    //     return parts;
    // }

    public getObject(objectId: string) {
        for (var object of this.roomObjects) {
            if (object.name == objectId) {
                return object;
            }
        }
        return null;
    }

    public getActor(actorId: string) : Actor {
        for (var object of this.roomObjects) {
            if (object.name == "actor-" + actorId) {
                return <Actor>object;
            }
        }
        return null;
    }

    public addObject(object: RoomObject, x: number, y: number, foreground: boolean) {

        // Dirty hack: when an object needs to be shown in the foreground, place it in
        // the actor layer.
        var layer = foreground ? this.layers.actors : this.layers.objects;

        object.create(this.game, this.uiMediator, x, y, layer);

        this.roomObjects.push(object);
    }

    public addActor(object: RoomObject, x: number, y: number) {
        
        object.create(this.game, this.uiMediator, x, y, this.layers.actors);


        this.roomObjects.push(object);
    }

    public removeObject(object: RoomObject) {

        // The object no longer needs a visual representation in the room.
        object.kill();

        var index = this.roomObjects.indexOf(object);
        this.roomObjects.splice(index, 1);
    }

    public kill() {
        for (var object of this.roomObjects) {
            object.kill();
        }
    }

    // function reconstruct_path(cameFrom, current)
    // total_path := {current}
    // while current in cameFrom.Keys:
    //     current := cameFrom[current]
    //     total_path.append(current)
    // return total_path


}
