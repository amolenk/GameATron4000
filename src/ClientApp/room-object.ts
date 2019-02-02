/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { UIMediator } from "./ui-mediator"

export class RoomObject {

    protected sprite: Phaser.Sprite;
    private stateMap: Map<string, any>;

    constructor(public name: string, public displayName: string) {
        this.stateMap = new Map<string, any>();
    }

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, group: Phaser.Group) {

        this.sprite = game.add.sprite(x, y, this.name);
        this.sprite.anchor.set(0.5);
        this.sprite.inputEnabled = true;
        this.sprite.input.pixelPerfectClick = true;
        this.sprite.input.pixelPerfectOver = true;

        group.add(this.sprite);

        this.sprite.events.onInputOver.add(() => uiMediator.focusObject(this));
        this.sprite.events.onInputOut.add(() => uiMediator.focusObject(null));
        this.sprite.events.onInputDown.add(() => uiMediator.selectObject(this));
    }

    public setVisible(visible: boolean) : void {
        this.sprite.visible = visible;
    }

    public kill() {
        this.sprite.destroy();
        this.sprite = null;
    }
}
