/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/rx/ts/rx.d.ts" />

import { Action } from "./action"
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

declare var gameInfo: any;

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
    }

    public create() {

        this.actionUI.create();
        this.verbsUI.create();

        this.setUIVisible(false);

        this.connectToBot();
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

    private connectToBot() {
        
        this.botClient.connect(
            async (message: any) => {
                
                if (message.actorId) {

                    var actor = this.room.getActor(message.actorId);

                    var match = /(.*?) \> (.*)/.exec(message.text);
                    if (match) {

                        await actor.say(match[2]);

                        if (message.suggestedActions) {
                            this.conversationUI.displaySuggestedActions(
                                this.room.getActor(gameInfo.playerActor),
                                message.suggestedActions.actions);
                        }
                    }
                }
            },
            async (event: any) => {
                
                switch (event.name) {
                    
                    case "ActorMoved": {
                        var actor = this.room.getActor(event.actorId);
                        await actor.walkTo(event.x, event.y);

                        // After actor has moved, change the direction the actor faces.
                        actor.changeDirection(event.direction);
                        break;
                    }

                    case "ActorDirectionChanged": {
                        var actor = this.room.getActor(event.actorId);
                        actor.changeDirection(event.direction);
                        break;
                    }

                    case "ActorPlacedInRoom": {
                        this.room.addActor(
                            new Actor("actor-" + event.actorId, event.description, event.textColor, event.direction),
                            event.x,
                            event.y);
                        break;
                    }

                    case "CloseUpOpened": {
                        var roomObject = new RoomObject("closeup-" + event.closeUpId, "");
                        this.room.addActor(roomObject, 400, 300);
                        break;
                    }

                    case "CloseUpClosed": {
                        var roomObject = this.room.getObject("closeup-" + event.closeUpId);
                        if (roomObject) {
                            this.room.removeObject(roomObject);
                        }
                        break;
                    }

                    case "Delayed": {
                        await new Promise(resolve => setTimeout(resolve, event.time));
                        break;
                    }

                    case "GameStarted": {
                        for (var inventoryItem of event.inventoryItems) {
                            await this.inventoryUI.addToInventory(inventoryItem.inventoryItemId, inventoryItem.description);
                        }
                        break;
                    }

                    case "InventoryItemAdded": {
                        await this.inventoryUI.addToInventory(event.inventoryItemId, event.description);
                        break;
                    }

                    case "InventoryItemRemoved": {
                        await this.inventoryUI.removeFromInventory(event.inventoryItemId);
                        break;
                    }

                    case "Narrated": {
                        await this.room.narrator.say(event.text);
                        break;
                    }

                    case "ObjectPlacedInRoom": {
                        this.room.addObject(
                            new RoomObject("object-" + event.objectId, event.description),
                            event.x,
                            event.y,
                            event.foreground);
                        break;
                    }

                    case "ObjectRemovedFromRoom": {
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
                        this.game.lockRender = true;
                        this.room = new Room(event.roomId);
                        this.room.create(this.game, this, this.layers);

                        for (var gameActor of event.actors) {
                            this.room.addActor(
                                new Actor("actor-" + gameActor.actorId, gameActor.description, gameActor.textColor, gameActor.direction),
                                gameActor.x,
                                gameActor.y);
                        }

                        for (var gameObject of event.objects) {
                            this.room.addObject(
                                new RoomObject("object-" + gameObject.objectId, gameObject.description),
                                gameObject.x,
                                gameObject.y,
                                gameObject.foreground);
                        }

                        this.game.lockRender = false;
                        break;
                    }

                    case "Idle": {

                        // Always let the player actor face front after the actions have executed.
                        var player = this.room.getActor(gameInfo.playerActor);
                        player.changeDirection('Front');

                        this.setUIVisible(true);
                        break;
                    }
                }    
            }
        )
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