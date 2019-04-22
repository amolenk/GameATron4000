/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { RoomObject } from "./room-object"
import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"
import { InventoryItem } from "./inventory-item";

export class InventoryUI {

    private items: Array<InventoryItem>;
    private visible: boolean; 

    constructor(private game: Phaser.Game, private uiMediator: UIMediator, private layers: Layers) {
        this.items = new Array<InventoryItem>();
        this.visible = true;
    }

    public addToInventory(item: InventoryItem) {

        item.create(this.game, this.uiMediator, 400 + (42 * this.items.length), 476, this.layers.ui);
        item.setVisible(this.visible); // TODO necessary?

        this.items.push(item);

        return Promise.resolve();
    }

    public removeFromInventory(objectId: string) {
        
        var index = this.items.findIndex(i => i.id == objectId);
        if (index > -1) {
            this.items[index].kill();
            this.items.splice(index, 1);

            // reorder items
            for (let i = 0; i < this.items.length; i++) {
                this.items[i].setPosition(400 + (42 * i), 476);
            }
        }

        return Promise.resolve();
    }

    public setVisible(visible: boolean) {
        this.visible = visible;
        for (let item of this.items) {
            item.setVisible(visible);
        }
    }
}