/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/rx/ts/rx.d.ts" />

import { Action } from "./action"
import { ActionUI } from "./ui-action"
import { Actor } from "./actor"
import { BotClient } from "./botclient"
import { ConversationUI } from "./ui-conversation"
import { Cursor } from "./cursor"
import { InventoryItem } from "./inventory-item"
import { InventoryUI } from "./ui-inventory"
import { Layers } from "./layers"
import { Narrator } from "./narrator"
import { Room } from "./room"
import { RoomObject } from "./room-object"
import { Settings } from "./settings"
import { VerbsUI } from "./ui-verbs"

export class UIMediator {

    private actionUI: ActionUI;
    private conversationUI: ConversationUI;
    private inventoryUI: InventoryUI;
    private verbsUI: VerbsUI;

    private selectedAction: Action;
    private focussedObject: RoomObject;

    private room: Room;

    constructor(private game: Phaser.Game, private cursor: Cursor, private botClient: BotClient, private layers: Layers) {
        this.actionUI = new ActionUI(game, layers);
        this.conversationUI = new ConversationUI(game, botClient, layers);
        this.inventoryUI = new InventoryUI(game, this, layers);
        this.verbsUI = new VerbsUI(game, this, layers);

        botClient.connect(
            async (message: any) => {

                if (message.from.name != 'narrator') {
                    var actor = this.room.getActor(message.from.name);
                    await actor.say(message.text);

                    if (message.suggestedActions) {
                        this.conversationUI.displaySuggestedActions(
                            this.room.getActor("player"),
                            message.suggestedActions.actions);
                    }
                }
                else {
                    await this.room.narrator.say(message.text);
                }
            },
            async (event: any) => {
                
                switch (event.name) {
                    
                    case "ActorMoved": {
                        var actor = this.room.getActor(event.actorId);
                        await actor.walkTo(event.x, event.y);
                        break;
                    }

                    case "ActorFacedAway": {
                        var actor = this.room.getActor(event.actorId);
                        await actor.faceBack();
                        break;
                    }

                    case "ActorFacedFront": {
                        var actor = this.room.getActor(event.actorId);
                        await actor.faceFront();
                        break;
                    }

                    case "CloseUpOpened": {
                        var roomObject = new RoomObject("closeup-" + event.objectId, "");
                        this.room.addActor(roomObject, 400, 300);
                        break;
                    }

                    case "CloseUpClosed": {
                        var roomObject = this.room.getObject("closeup-" + event.objectId);
                        if (roomObject) {
                            this.room.removeObject(roomObject);
                        }
                        break;
                    }

                    case "Delayed": {
                        await new Promise(resolve => setTimeout(resolve, event.time));
                        break;
                    }

                    case "InventoryItemAdded": {
                        // Remove the object from the room (if it currently exists in a room).
                        if (this.room) {
                            var roomObject = this.room.getObject("object-" + event.objectId);
                            if (roomObject) {
                                this.room.removeObject(roomObject);
                            }
                        }
                        await this.inventoryUI.addToInventory(event.objectId, event.description);
                        break;
                    }

                    case "InventoryItemRemoved": {
                        await this.inventoryUI.removeFromInventory(event.objectId);
                        break;
                    }

                    case "RoomObjectAdded": {
                        var roomObject = new RoomObject("object-" + event.objectId, event.description);
                        if (event.foreground) {
                            this.room.addActor(roomObject, event.x, event.y);                            
                        } else {
                            this.room.addObject(roomObject, event.x, event.y);
                        }
                        break;
                    }

                    case "RoomObjectRemoved": {
                        var roomObject = this.room.getObject("object-" + event.objectId);
                        if (roomObject) {
                            this.room.removeObject(roomObject);
                        }
                        break;
                    }

                    case "RoomEntered": {
                        if (this.room != null) {
                            this.room.kill();
                        }
                        this.room = new Room(event.roomId);
                        this.room.create(game, this, this.layers, event.objects, event.actors);
                        break;
                    }

                    case "Idle": {
                        this.setUIVisible(true);
                        break;
                    }
                }    
            }
        )
    }

    public create() {

        this.actionUI.create();
        this.verbsUI.create();

        this.setUIVisible(false);
    }

    public selectAction(action: Action) {
        this.selectedAction = action;
        this.updateText();
    }

    public focusObject(roomObject: RoomObject) {
        this.focussedObject = roomObject;
        this.updateText();
    }

    public selectObject(roomObject: RoomObject) {
        if (this.selectedAction != null) {
            if (this.selectedAction.addSubject(roomObject)) {

                // If the action is complete it can be executed.
                this.setUIVisible(false);
                this.botClient.sendActionToBot(this.selectedAction);
                
                // Set the selected action to null now that we've executed it.
                // Also set the object that the mouse is hovering over to null.
                // This will prevent the verb bar from showing the object display name
                // until the mouse has left and re-entered the object. Much less
                // distracting for the player.
                this.selectedAction = null;
                this.focussedObject = null;
            }
        }

        this.updateText();
    }

    private setUIVisible(visible: boolean) {
        this.actionUI.setVisible(visible);
        this.inventoryUI.setVisible(visible);
        this.verbsUI.setVisible(visible);
    }

    private updateText() {

        var text = "";

        if (this.selectedAction != null) {
            text = this.selectedAction.getDisplayText(this.focussedObject);
        }
        else if (this.focussedObject != null) {
            text = this.focussedObject.displayName;
        }

        this.actionUI.setText(text);
    }
}