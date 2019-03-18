/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Actor } from "./actor"
import { Layers } from "./layers"
import { Narrator } from "./narrator"
import { RoomObject } from "./room-object"
import { UIMediator } from "./ui-mediator"
import { Graph } from "./graph"
import { Walkbox } from "./walkbox";

export class Room {

    public narrator: Narrator;

    private game: Phaser.Game;
    private roomObjects: Array<RoomObject>;
    private actionMap: Map<string, Function>;
    private uiMediator: UIMediator;
    private layers: Layers;
    private graphics: Phaser.Graphics;
    private walkbox: Walkbox;

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

//       var background = this.game.add.sprite(0, 0, "room-" + this.name);
//       background.inputEnabled = true;
//        TODO Temp?
        // background.events.onInputDown.add((args: any, pointer: Phaser.Pointer) => {
        //     this.temp(pointer.positionDown);
        // });

//        this.layers.background.add(background);

        this.narrator = new Narrator(game, layers);
        this.narrator.create();

        // TODO Does Phaser close the polygon automatically?
        const polygons = [];
        polygons.push(new Phaser.Polygon(
            new Phaser.Point(799, 100),
            new Phaser.Point(799, 318),
            new Phaser.Point(590, 364),
            new Phaser.Point(799, 416),
            new Phaser.Point(799, 449),
            new Phaser.Point(568, 449),
            new Phaser.Point(380, 375),
            new Phaser.Point(250, 400), // TODO Remove this point and it breaks (concerns snapping)
            new Phaser.Point(100, 300),
            new Phaser.Point(0, 324),
            new Phaser.Point(0, 316),
            new Phaser.Point(130, 150),
            new Phaser.Point(250, 320),
            new Phaser.Point(380, 150),
            new Phaser.Point(799, 100)
        ));
        polygons.push(new Phaser.Polygon(
            new Phaser.Point(420, 250),
            new Phaser.Point(470, 250),
            new Phaser.Point(495, 300),
            new Phaser.Point(470, 350),
            new Phaser.Point(420, 350),
            new Phaser.Point(395, 300)
        ));
        this.walkbox = new Walkbox(polygons);

        
    }

    public debug() {
        this.walkbox.debug(700, 420, this.game.input.x, this.game.input.y, this.game.debug);
    }

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
}
