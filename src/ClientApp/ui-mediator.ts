/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/rx/ts/rx.d.ts" />

import { IAction, WalkToAction } from "./action"
import { ActionUI } from "./ui-action"
import { Actor } from "./actor"
import { BotClient } from "./botclient"
import { ConversationUI } from "./ui-conversation"
import { Cursor } from "./cursor"
import { InventoryUI } from "./ui-inventory"
import { Layers } from "./layers"
import { Room } from "./room"
import { RoomObject } from "./room-object"
import { VerbsUI } from "./ui-verbs"
import { InventoryItem } from "./inventory-item";

declare var gameInfo: any;

export class UIMediator {
    
    private actionUI: ActionUI;
    private conversationUI: ConversationUI;
    private inventoryUI: InventoryUI;
    private verbsUI: VerbsUI;

    private selectedAction: IAction;
    private focussedObject: RoomObject | InventoryItem | Actor;

    private room: Room;
    private _selectedActor: string;

    constructor(private game: Phaser.Game, private cursor: Cursor, private botClient: BotClient, private layers: Layers) {
        this.actionUI = new ActionUI(game, layers);
        this.conversationUI = new ConversationUI(game, botClient, layers);
        this.inventoryUI = new InventoryUI(game, this, layers);
        this.selectedAction = new WalkToAction();
        this.verbsUI = new VerbsUI(game, this, layers);
    }

    get selectedActor(): string {
        return this._selectedActor;
    }

    public create() {

        this.actionUI.create();
        this.verbsUI.create();

        this.setUIVisible(false);

        this.connectToBot();
    }

    public selectAction(action: IAction) {
        this.selectedAction = action;
        this.updateText();
    }

    public focusObject(roomObject: RoomObject | InventoryItem | Actor) {
        this.focussedObject = roomObject;
        this.updateText();
    }

    public async selectObject(target: RoomObject | InventoryItem | Actor) {
        
        if (this.selectedAction != null) {
            if (this.selectedAction.addSubject(target)) {

                // If the action is complete it can be executed.
                this.setUIVisible(false);

                const action = this.selectedAction;

                var actor = this.room.getActor(this._selectedActor);

                if (!(target instanceof InventoryItem)
                    && target.usePosition != "none")
                {
                    const walkToX = target.x;
                    let walkToY = target.y;

                    if (target.usePosition == "infront") {
                        walkToY += 20; // TODO Consider scaling
                    } else if (target.usePosition == "above") {
                        walkToY -= 20;
                    }

                    await this.room.moveActor(this._selectedActor, walkToX, walkToY, target.useDirection);
                }

                await this.botClient.sendActionToBot(action, actor);

                // Set the selected action to null now that we've executed it.
                // Also set the object that the mouse is hovering over to null.
                // This will prevent the verb bar from showing the object display name
                // until the mouse has left and re-entered the object. Much less
                // distracting for the player.
                this.selectedAction = null;
                this.focussedObject = null;
            }
        }
        else
        {
            if (!(target instanceof InventoryItem))
            {
                await this.room.moveActor(this._selectedActor, target.x, target.y, "front");
            }
        }

        this.updateText();
    }

    public update() {
        if (this.room) {
            this.room.update();
        }
    }

    public debug() {
        if (this.room) {
            this.room.debug();
        }
    }

    private connectToBot() {
        
        this.botClient.connect(
            async (message: any) => {
                
                if (message.actor) {

                    var actor = this.room.getActor(message.actor.id);

                    var match = /(.*?) \> (.*)/.exec(message.text);
                    if (match) {

                        if (match[2]) {
                            await actor.sayLine(match[2]);
                        }

                        if (message.suggestedActions) {
                            this.conversationUI.displaySuggestedActions(
                                this.room.getActor(this._selectedActor),
                                message.suggestedActions.actions);
                        }
                    }
                } else if (message.narrator) {
                    await this.room.narrator.say(message.text);
                }
            },
            async (event: any) => {
                
                switch (event.name) {
                    
                    case "ActorMoved": {
                        await this.room.moveActor(event.actor.id, event.actor.x, event.actor.y, event.actor.faceDirection);
                        break;
                    }

                    case "ActorDirectionFacedChanged": {
                        var actor = this.room.getActor(event.actor.id);
                        actor.changeDirection(event.actor.direction);
                        break;
                    }

                    case "ActorPlacedInRoom": {
                        this.room.addActor(
                            new Actor(
                                event.actor.id,
                                event.actor.name,
                                event.actor.classes,
                                event.actor.usePosition,
                                event.actor.useDirection,
                                event.actor.faceDirection,
                                event.actor.textColor),
                            event.actor.x,
                            event.actor.y);
                        break;
                    }

                    case "CameraFocusChanged": {
                        var actor = this.room.getActor(event.actor.id);
                        actor.focusCamera(false);
                        break;
                    }

                    case "ErrorOccured": {

                        // TODO Extract
                        var textStyle = {
                            font: "54px Onesize", // Using a large font-size and scaling it back looks better.
                            fill: "red",
                            stroke: "black",
                            strokeThickness: 12,
                            align: "center",
                            wordWrap: "true",
                            wordWrapWidth: 1400 // Account for scaling.
                        };

                        this.game.camera.shake(0.05, 500);
                        this.game.camera.flash(0xff0000, 500);

                        var errorText = this.game.add.text(400, 150, event.message, textStyle);
                        errorText.anchor.set(0.5);                        
                        errorText.lineSpacing = -30;
                        errorText.scale.x = 0.5;
                        errorText.scale.y = 0.5;
                        errorText.width = 600;
                        errorText.fixedToCamera = true;

                        console.log(event.message);
                        console.log(event.stackTrace);
                        break;
                    }

                    case "GameStarted": {
                        this._selectedActor = event.actor.id;
                        // TODO Set up camera
                        for (let obj of event.inventory) {
                            await this.inventoryUI.addToInventory(
                                new InventoryItem(obj.id, obj.name, obj.classes));
                        }
                        break;
                    }

                    case "Halted": {
                        await new Promise(resolve => setTimeout(resolve, event.time));
                        break;
                    }

                    // case "ObjectPickedUp": {
                    //     // TODO Maybe the object isn't in the room yet.
                    //     // E.g. it's given to you from another actor.
                    //     const obj = this.room.getObject(event.object.id);
                    //     await this.inventoryUI.addToInventory(obj);
                    //     break;
                    // }

                    case "InventoryItemAdded": {
                        await this.inventoryUI.addToInventory(
                            new InventoryItem(event.item.id, event.item.name, event.item.classes));
                        break;
                    }

                    case "InventoryItemRemoved": {
                        await this.inventoryUI.removeFromInventory(event.item.id);
                        break;
                    }

                    case "ObjectPlacedInRoom": {
                        this.room.addObject(
                            new RoomObject(event.object.id, event.object.name, event.object.classes, event.object.state, event.object.usePosition, event.object.useDirection),
                            event.object.cam_offset ? event.object.x + this.game.camera.x: event.object.x,
                            event.object.y,
                            event.object.z_offset);
                        break;
                    }

                    case "ObjectRemovedFromRoom": {
                        var roomObject = this.room.getObject(event.object.id);
                        if (roomObject) {
                            this.room.removeObject(roomObject);
                        }
                        break;
                    }

                    case "ObjectStateChanged": {
                        this.game.lockRender = true;

                        const object = this.room.getObject(event.object.id);
                        if (object) {
                            object.changeState(event.object.state);
                        }

                        for (let obj of event.add) {
                            this.room.addObject(
                                new RoomObject(obj.id, obj.name, obj.classes, obj.state, obj.usePosition, obj.useDirection),
                                obj.x,
                                obj.y,
                                obj.z_offset);
                        }

                        for (let obj of event.remove) {
                            var roomObject = this.room.getObject(obj.id);
                            if (roomObject) {
                                this.room.removeObject(roomObject);
                            }
                        }

                        this.game.lockRender = false;
                        break;
                    }

                    case "RoomEntered": {
                        this.game.lockRender = true;
                        if (this.room != null) {
                            this.room.kill();
                        }

                        const walkbox = new Phaser.Polygon(
                            event.room.walkbox.map((p: any) => new Phaser.Point(p.x, p.y)));

                        this.room = new Room(event.room.id, event.room.scale, walkbox);
                        this.room.create(this.game, this, this.layers);

                        for (let actor of event.actors) {
                            this.room.addActor(
                                new Actor(
                                    actor.id,
                                    actor.name,
                                    actor.classes,
                                    actor.usePosition,
                                    actor.useDirection,
                                    actor.faceDirection,
                                    actor.textColor),
                                actor.x,
                                actor.y);
                        }

                        for (let obj of event.objects) {
                            this.room.addObject(
                                new RoomObject(obj.id, obj.name, obj.classes, obj.state, obj.usePosition, obj.useDirection),
                                obj.x,
                                obj.y,
                                obj.z_offset);
                        }

                        this.game.lockRender = false;
                        break;
                    }

                    case "Idle": {

                        // TODO Not anymore?
                        // Always let the player actor face front after the actions have executed.
                        // const actor = this.room.getActor(this._selectedActor);
                        // actor.changeDirection('front');
                        this.selectedAction = new WalkToAction();
                        this.setUIVisible(true);
                        break;
                    }
                }    
            }
        )
    }

    public uiEnabled: boolean = false;

    public setUIVisible(visible: boolean) {
        this.actionUI.setVisible(visible);
        this.inventoryUI.setVisible(visible);
        this.verbsUI.setVisible(visible);
        this.uiEnabled = visible;
    }

    private updateText() {

        var text = "Walk to";

        if (this.selectedAction != null) {
            text = this.selectedAction.getDisplayText(this.focussedObject);
        }
        else if (this.focussedObject != null) {
            text = `Walk to ${this.focussedObject.name}`;
        }

        this.actionUI.setText(text);
    }
}