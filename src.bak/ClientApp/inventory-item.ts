import { UIMediator } from "./ui-mediator"

export class InventoryItem {

    protected sprite: Phaser.Sprite;

    constructor(public id: string, public name: string, public classes: string[]) {
    }

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, group: Phaser.Group) {

        this.sprite = game.add.sprite(x, y, "sprites", "inventory/" + this.id);
        this.sprite.anchor.set(0);
        this.sprite.inputEnabled = true;
        this.sprite.fixedToCamera = true;

        group.add(this.sprite);

        // TODO
        this.sprite.events.onInputOver.add(() => uiMediator.focusObject(this));
        this.sprite.events.onInputOut.add(() => uiMediator.focusObject(null));
        this.sprite.events.onInputDown.add(() => uiMediator.selectObject(this));
    }

    public setPosition(x: number, y: number) {
        this.sprite.fixedToCamera = false;
        this.sprite.x = x;
        this.sprite.y = y;
        this.sprite.fixedToCamera = true;
    }       

    public setVisible(visible: boolean) : void {
        this.sprite.visible = visible;
    }

    public kill() {
        this.sprite.destroy();
        this.sprite = null;
    }
}

