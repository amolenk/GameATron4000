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

    constructor(private name: string) {
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

        var background = this.game.add.sprite(0, 0, "sprites", "rooms/park");// + this.name);
        background.inputEnabled = true;

        // Update world bounds to size of room.
        this.game.world.setBounds(0, 0, background.width, this.game.world.height);

        // Whenever the player clicks on the background, move the actor to
        // that point.
        background.events.onInputDown.add(() =>
            this.moveActor("player", this.game.input.x + this.game.camera.x, this.game.input.y + this.game.camera.y, "front"));

        this.layers.background.add(background);

        this.narrator = new Narrator(game, layers);
        this.narrator.create();

        // const bench = this.game.add.sprite(323, 406, 'sprites', 'Objects/park-bench', this.layers.actors);
        // bench.anchor.set(0.5, 1);

        // const actor = new Actor("actor-guy", "guy", "White", "Front");
        // this.addActor(actor, 640, 430);

        // const al = new Actor("actor-al", "al", "Yellow", "Front");
        // this.addActor(al, 520, 385);
        // al.say("Let's do this!");

        // TODO Does Phaser close the polygon automatically?
        this.walkboxPolygons = [];
        this.walkboxPolygons.push(new Phaser.Polygon(
            new Phaser.Point(935, 295),
            new Phaser.Point(935, 318),
            new Phaser.Point(710, 375),
            new Phaser.Point(710, 385),
            new Phaser.Point(935, 442),
            new Phaser.Point(935, 449),
            new Phaser.Point(591, 449),
            new Phaser.Point(393, 375),
            new Phaser.Point(108, 336),
            new Phaser.Point(0, 330),
            new Phaser.Point(0, 312),
            new Phaser.Point(130, 315),
            new Phaser.Point(366, 338),

            new Phaser.Point(470, 355),
            new Phaser.Point(804, 316)));



//         polygons.push(new Phaser.Polygon(
//             new Phaser.Point(799, 100),
//             new Phaser.Point(799, 318),
//             new Phaser.Point(590, 364),
//             new Phaser.Point(799, 416),
//             new Phaser.Point(799, 449),
//             new Phaser.Point(568, 449),
//             new Phaser.Point(380, 375),
// //            new Phaser.Point(250, 400), // TODO Remove this point and it breaks (concerns snapping)
//             new Phaser.Point(100, 300),
//             new Phaser.Point(0, 324),
//             new Phaser.Point(0, 316),
//             new Phaser.Point(130, 150),
//             new Phaser.Point(250, 320),
//             new Phaser.Point(380, 150),
//             new Phaser.Point(799, 100)
//         ));
//         polygons.push(new Phaser.Polygon(
//             new Phaser.Point(420, 250),
//             new Phaser.Point(470, 250),
//             new Phaser.Point(495, 300),
//             new Phaser.Point(470, 350),
//             new Phaser.Point(420, 350),
//             new Phaser.Point(395, 300)
//         ));

        
    }

    public update() {
        for (let actor of this.actors) {
            actor.update();
        }
    }

    public debug() {
        const actor = this.getActor("player");
        const walkbox = this.createWalkbox();
        walkbox.debug(actor.x, actor.y, this.game.input.x + this.game.camera.x, this.game.input.y + this.game.camera.y, this.game.debug);
    }

    public getObject(objectId: string) {
        for (var object of this.roomObjects) {
            if (object.name == objectId) {
                return object;
            }
        }
        return null;
    }

    public addActor(actor: Actor, x: number, y: number) {
        
        actor.create(this.game, this.uiMediator, x, y, this.layers.objects);
        this.actors.push(actor);

        // When the player actor is added to the room, follow it
        // with the camera.
        if (actor.name == "player") {
            //  0.1 is the amount of linear interpolation to use.
            //  The smaller the value, the smooth the camera (and the longer it takes to catch up)
            this.game.camera.focusOn(actor.spriteDebug);
            this.game.camera.follow(actor.spriteDebug, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);
        }
    }

    public getActor(actorId: string): Actor {
        return this.actors.find(a => a.name == actorId);
    }

    public async moveActor(actorId: string, toX: number, toY: number, faceDirection: string) {
        const actor = this.getActor(actorId);
        const walkbox = this.createWalkbox();
        const path = walkbox.findPath(actor.x, actor.y, toX, toY);
        if (path) {
            path.shift();
            await actor.moveTo(path);
        }
    }

    public addObject(object: RoomObject, x: number, y: number) { //}, foreground: boolean) {

        // Dirty hack: when an object needs to be shown in the foreground, place it in
        // the actor layer.
//        var layer = foreground ? this.layers.actors : this.layers.objects;
        var layer = this.layers.objects;

        object.create(this.game, this.uiMediator, x, y, layer);

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

    private createWalkbox(): Walkbox {

        const actorPolygons = this.actors
            .filter(actor => actor instanceof Actor && actor.name != "player")
            .map(actor => new Phaser.Polygon(
                new Phaser.Point(actor.x - 40, actor.y - 10),
                new Phaser.Point(actor.x + 40, actor.y - 10),
                new Phaser.Point(actor.x + 40, actor.y + 10),
                new Phaser.Point(actor.x - 40, actor.y + 10)));

        const polygons = this.walkboxPolygons.concat(actorPolygons);

        return new Walkbox(polygons);
    }
}
