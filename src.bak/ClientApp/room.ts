/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Actor } from "./actor"
import { Layers } from "./layers"
import { Narrator } from "./narrator"
import { RoomObject } from "./room-object"
import { UIMediator } from "./ui-mediator"
import { Walkbox } from "./walkbox";

export class Room {

    public narrator: Narrator;

    private game: Phaser.Game;
    private roomObjects: Array<RoomObject>;
    private actors: Array<Actor>;
    private actionMap: Map<string, Function>;
    private uiMediator: UIMediator;
    private layers: Layers;
    private graphics: Phaser.Graphics;
    private walkboxPolygons: Phaser.Polygon[];

    constructor(private name: string, private scale: any, private walkbox: Phaser.Polygon) {
        this.roomObjects = new Array<RoomObject>();
        this.actors = new Array<Actor>();
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

        this.actors = [];

        var background = this.game.add.sprite(0, 0, "sprites", "rooms/" + this.name);
        background.inputEnabled = true;

        // Update world bounds to size of room.
        this.game.world.setBounds(0, 0, background.width, this.game.world.height);

        // Whenever the player clicks on the background, move the actor to
        // that point.
        background.events.onInputDown.add(() => {
            if (this.uiMediator.uiEnabled) {
                this.moveActor(this.uiMediator.selectedActor, this.game.input.x + this.game.camera.x, this.game.input.y + this.game.camera.y, "front")
            }});

        this.layers.background.add(background);

        this.narrator = new Narrator(game, layers);
        this.narrator.create();

        this.walkboxPolygons = [];
        this.walkboxPolygons.push(this.walkbox);
    }

    public update() {
        for (let actor of this.actors) {
            actor.update();
        }
    }

    public debug() {
        const actor = this.getActor(this.uiMediator.selectedActor);
        const walkbox = this.createWalkbox(actor);
        walkbox.debug(actor.x, actor.y, this.game.input.x + this.game.camera.x, this.game.input.y + this.game.camera.y, this.game.debug);
    }

    public getObject(objectId: string) {
        for (var object of this.roomObjects) {
            if (object.id == objectId) {
                return object;
            }
        }
        return null;
    }

    public addActor(actor: Actor, x: number, y: number) {
        
        actor.create(this.game, this.uiMediator, x, y, this.layers, this.scale);
        this.actors.push(actor);

        // When the player actor is added to the room, follow it
        // with the camera.
        if (actor.id == this.uiMediator.selectedActor) {
            //  0.1 is the amount of linear interpolation to use.
            //  The smaller the value, the smooth the camera (and the longer it takes to catch up)
            actor.focusCamera(true)
        }
    }

    public getActor(actorId: string): Actor {
        return this.actors.find(a => a.id == actorId);
    }

    public async moveActor(actorId: string, toX: number, toY: number, faceDirection: string) {
        const actor = this.getActor(actorId);
        const walkbox = this.createWalkbox(actor);
        const path = walkbox.findPath(actor.x, actor.y, toX, toY);
        if (path) {
            path.shift();
            await actor.walkTo(path, faceDirection);
        } else {
            actor.changeDirection(faceDirection);
        }
    }

    public addObject(object: RoomObject, x: number, y: number, zOffset: number) {

        object.create(this.game, this.uiMediator, x, y, zOffset, this.layers.objects);

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
        for (var actor of this.actors) {
            actor.kill();
        }
    }

    private createWalkbox(forActor: Actor): Walkbox {

        const actorPolygons = this.actors
            .filter(actor => actor instanceof Actor && actor.id != forActor.id)
            .map(actor => new Phaser.Polygon( // TODO Consider scaling
                new Phaser.Point(actor.x - 50, actor.y - 20),
                new Phaser.Point(actor.x + 50, actor.y - 20),
                new Phaser.Point(actor.x + 50, actor.y + 20),
                new Phaser.Point(actor.x - 50, actor.y + 20)));

        const polygons = this.walkboxPolygons.concat(actorPolygons);

        return new Walkbox(polygons);
    }
}
