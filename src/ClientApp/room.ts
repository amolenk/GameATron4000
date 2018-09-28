/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Actor } from "./actor"
import { Layers } from "./layers"
import { Narrator } from "./narrator"
import { RoomObject } from "./room-object"
import { UIMediator } from "./ui-mediator"

export class Room {

    public narrator: Narrator;

    private game: Phaser.Game;
    private roomObjects: Array<RoomObject>;
    private actionMap: Map<string, Function>;
    private uiMediator: UIMediator;
    private layers: Layers;

    constructor(private name: string) {
        this.roomObjects = new Array<RoomObject>();
        this.actionMap = new Map<string, Function>();
    }

    public create(
        game: Phaser.Game,
        uiMediator: UIMediator,
        layers: Layers,
        objects?: any,
        actors?: any) {
        
        this.game = game;
        this.uiMediator = uiMediator;
        this.layers = layers;

        var background = this.game.add.sprite(0, 0, "room-" + this.name);
        this.layers.background.add(background);

        if (objects) {
            for (var objectData of objects) {
                var object = new RoomObject("object-" + objectData.id, objectData.description);
                this.addObject(object, objectData.x, objectData.y);
            }
        }

        if (actors) {
            for (var actorData of actors) {
                var actor = new Actor("actor-" + actorData.id, actorData.description, actorData.textColor);
                this.addActor(actor, actorData.x, actorData.y);
            }
        }

        this.narrator = new Narrator(game, layers);
        this.narrator.create();
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

    public addObject(object: RoomObject, x: number, y: number) {

        object.create(this.game, this.uiMediator, x, y, this.layers.objects);

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
