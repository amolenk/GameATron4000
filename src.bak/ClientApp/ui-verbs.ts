/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { GiveAction, UnaryAction, UseAction } from "./action"
import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"

export class VerbsUI {

    private verbSprites: Array<Phaser.Sprite>;

    constructor(private game: Phaser.Game, private uiMediator: UIMediator, private layers: Layers) {
        this.verbSprites = new Array<Phaser.Sprite>();
    }

    public create() {
        
        this.addVerb("01_give", 0, 0, () => new GiveAction());
        this.addVerb("02_pickup", 1, 0, () => new UnaryAction("Pick up"));
        this.addVerb("03_use", 2, 0, () => new UseAction());
        this.addVerb("04_open", 0, 1, () => new UnaryAction("Open"));
        this.addVerb("05_lookat", 1, 1, () => new UnaryAction("Look at"));
        this.addVerb("06_push", 2, 1, () => new UnaryAction("Push"));
        this.addVerb("07_close", 0, 2, () => new UnaryAction("Close"));
        this.addVerb("08_talkto", 1, 2, () => new UnaryAction("Talk to"));
        this.addVerb("09_pull", 2, 2, () => new UnaryAction("Pull"));
    }

    public setVisible(visible: boolean) {
        for (var sprite of this.verbSprites) {
            sprite.visible = visible;
        }
    }

    private addVerb(
        id: string,
        posX: number,
        posY: number,
        actionFactory: Function) {

        var x = 4;
        if (posX > 0) x += (posX == 1 ? 100 : 260);
        var y = 476 + (posY * 40);

        var sprite = this.game.add.sprite(x, y, "verbs");
        sprite.frameName = id + ".png";
        sprite.inputEnabled = true;
        sprite.fixedToCamera = true;

        sprite.events.onInputOver.add(() => {
            sprite.frameName = id + "_sel.png";
        }, this);

        sprite.events.onInputOut.add(() => {
            sprite.frameName = id + ".png";
        }, this);

        sprite.events.onInputDown.add(() => {
            this.uiMediator.selectAction(actionFactory());
        }, this);

        this.layers.ui.add(sprite);
        this.verbSprites.push(sprite);
    }
}