/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { InventoryItem } from "./inventory-item"
import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"

export class InventoryUI {

    private items: Map<string, InventoryItem>;
    private visible: boolean; 

    constructor(private game: Phaser.Game, private uiMediator: UIMediator, private layers: Layers) {
        this.items = new Map<string, InventoryItem>();
        this.visible = true;
    }

    public addToInventory(objectId: string, description: string) {

        var item = new InventoryItem("inventory-" + objectId, description);

        item.create(this.game, this.uiMediator, 400 + (42 * this.items.size), 476, this.layers.ui);
        item.setVisible(this.visible);

        this.items.set(item.name, item);

        return Promise.resolve();
    }

    public removeFromInventory(objectId: string) {
        
        var item = this.items.get("inventory-" + objectId);
        if (item) {
            item.kill();
            this.items.delete(item.name);
        }

        // reorder itemss
        var itemIndex = 0;
        this.items.forEach((value: InventoryItem, key: string) => {
            value.setPosition(400 + (42 * itemIndex), 476);
            itemIndex++;
        });

        return Promise.resolve();
    }

    public setVisible(visible: boolean) {
        this.visible = visible;
        for (var item of this.items.values()) {
            item.setVisible(visible);
        }
    }
}