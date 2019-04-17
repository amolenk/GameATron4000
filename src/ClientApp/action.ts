/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { Actor } from "./actor"
import { RoomObject } from "./room-object"
import { InventoryItem } from "./inventory-item";

export interface IAction {
    addSubject: (subject: RoomObject | InventoryItem | Actor) => boolean;
    getDisplayText: (subject?: RoomObject | InventoryItem | Actor) => string;
}

export class UnaryAction implements IAction {

    private _objectOrActor: RoomObject | InventoryItem | Actor;

    constructor(private verb: string) {
    }

    public addSubject(subject: RoomObject | InventoryItem | Actor) {

        this._objectOrActor = subject;
        return true;
    }

    public getDisplayText(subject?: RoomObject | InventoryItem | Actor) {

        if (this._objectOrActor) {
            return `${this.verb} ${this._objectOrActor.name}`;
        }

        if (subject) {
            return `${this.verb} ${subject.name}`;
        }

        return this.verb;
    }
}

export class GiveAction implements IAction {

    private _inventoryItem: InventoryItem;
    private _actor: Actor;

    public addSubject(subject: RoomObject | InventoryItem | Actor) {

        if (this._inventoryItem)
        {
            if (subject instanceof Actor) {
                this._actor = subject;
                return true;
            }
        }
        else
        {
            if (subject instanceof InventoryItem) {
                this._inventoryItem = subject;
            }
        }

        return false;
    }

    public getDisplayText(subject?: RoomObject | InventoryItem | Actor) {

        if (this._inventoryItem)
        {
            if (this._actor) {
                return `Give ${this._inventoryItem.name} to ${this._actor.name}`;
            }

            if (subject instanceof Actor) {
                return `Give ${this._inventoryItem.name} to ${subject.name}`;
            }

            return `Give ${this._inventoryItem.name} to`;
        }

        if (subject instanceof InventoryItem) {
            return `Give ${subject.name}`;
        }

        return "Give";
    }
}

export class UseAction implements IAction {

    private _inventoryItemOrObject1: RoomObject | InventoryItem;
    private _inventoryItemOrObject2: RoomObject | InventoryItem;

    public addSubject(subject: RoomObject | InventoryItem | Actor) {

        if (this._inventoryItemOrObject1 && this.isUseWith())
        {
            if ((subject instanceof RoomObject || subject instanceof InventoryItem)
                && subject != this._inventoryItemOrObject1) {
                this._inventoryItemOrObject2 = subject;
                return true;
            }
        }
        else
        {
            if (subject instanceof RoomObject || subject instanceof InventoryItem) {
                this._inventoryItemOrObject1 = subject;
                return !this.isUseWith();
            }
        }

        return false;
    }

    public getDisplayText(subject?: RoomObject | InventoryItem | Actor) {

        if (this._inventoryItemOrObject1)
        {
            // Ignore the current room object that the mouse is over if it's the same as
            // the first subject (if any).
            if (subject == this._inventoryItemOrObject1) {
                subject = null;
            }

            if (this._inventoryItemOrObject2) {
                return `Use ${this._inventoryItemOrObject1.name} with ${this._inventoryItemOrObject2.name}`;
            }

            if (subject instanceof RoomObject || subject instanceof InventoryItem) {
                return `Use ${this._inventoryItemOrObject1.name} with ${subject.name}`;
            }

            if (this.isUseWith()) {
                return `Use ${this._inventoryItemOrObject1.name} with`;
            }

            return `Use ${this._inventoryItemOrObject1.name}`;
        }

        if (subject instanceof RoomObject || subject instanceof InventoryItem) {
            return `Use ${subject.name}`;
        }

        return "Use";
    }

    private isUseWith() : boolean
    {
        return this._inventoryItemOrObject1 instanceof InventoryItem
            && this._inventoryItemOrObject1.classes.indexOf("class_use_with") > -1;
    }
}