/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { RoomObject } from "./room-object"
import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"
import { InventoryItem } from "./inventory-item";

export class InventoryUI {

    private objects: Map<string, InventoryItem>; // TODO Rename to items
    private visible: boolean; 

    constructor(private game: Phaser.Game, private uiMediator: UIMediator, private layers: Layers) {
        this.objects = new Map<string, InventoryItem>();
        this.visible = true;
    }

    public addToInventory(item: InventoryItem) {//} objectId: string, description: string, classes: string[]) {

        item.create(this.game, this.uiMediator, 400 + (42 * this.objects.size), 476, this.layers.ui);
        item.setVisible(this.visible); // TODO necessary?

        this.objects.set(item.id, item);

        return Promise.resolve();
    }

    public removeFromInventory(objectId: string) {
        
        var item = this.objects.get(objectId);
        if (item) {
            item.kill();
            this.objects.delete(item.id);

            // reorder itemss
            var itemIndex = 0;
            this.objects.forEach((value: InventoryItem, key: string) => {
                value.setPosition(400 + (42 * itemIndex), 476);
                itemIndex++;
            });
        }

        return Promise.resolve();
    }

    public setVisible(visible: boolean) {
        this.visible = visible;
        for (var item of this.objects.values()) {
            item.setVisible(visible);
        }
    }
}