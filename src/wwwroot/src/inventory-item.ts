import { RoomObject } from "./room-object"
import { UIMediator } from "./ui-mediator"

export class InventoryItem extends RoomObject {

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, group: Phaser.Group) {

        super.create(game, uiMediator, x, y, group);

        this.sprite.anchor.set(0);
    }

    public setPosition(x: number, y: number) {
        this.sprite.x = x;
        this.sprite.y = y;
    }       
}
