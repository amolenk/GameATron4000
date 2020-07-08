/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />
/// <reference path="../node_modules/typescript/lib/lib.es6.d.ts" />

import { UIMediator } from "./ui-mediator"

export class RoomObject {

    protected sprite: Phaser.Sprite;
    private _created: boolean = false;

    constructor(public id: string, public name: string, public classes: string[], private state: string, public usePosition: string, public useDirection: string) {
    }

    public get x() {
        return this.sprite.x;
    }

    public get y() {
        return this.sprite.y;
    }

    public get created() {
        return this._created;
    }

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, zOffset: number, group: Phaser.Group) {

        this.sprite = game.add.sprite(x, y, "sprites");
        this.sprite.anchor.set(0.5, 1);

        this.sprite.data.z = y + zOffset;

        // TODO Make animations configurable on objects, for now automagically
        // register the 'smoke' animation for the 'cooker.
        if (this.id == "cooker") {
            this.sprite.animations.add("smoke",
                Phaser.Animation.generateFrameNames(`objects/${this.id}/smoke/`, 1, 3, '', 4), 6, true, false);
        }

        this.updateFrame();

        group.add(this.sprite);

        if (this.classes.indexOf("class_fixed_to_camera") != -1) {
            this.sprite.fixedToCamera = true;
        }

        if (this.classes.indexOf("class_untouchable") == -1) {
            this.sprite.inputEnabled = true;
            //this.sprite.input.pixelPerfectClick = true;
            //this.sprite.input.pixelPerfectOver = true;

            this.sprite.events.onInputOver.add(() => uiMediator.focusObject(this));
            this.sprite.events.onInputOut.add(() => uiMediator.focusObject(null));
            this.sprite.events.onInputDown.add(() => uiMediator.selectObject(this));
        }

        this._created = true;
    }

    public changeState(state: string) {
        if (this.sprite.animations.getAnimation(this.state) != null) {
            this.sprite.animations.stop(this.state);
        }
        this.state = state;
        this.updateFrame();
    }

    public setPosition(x: number, y: number) {
        this.sprite.x = x;
        this.sprite.y = y;
    }  

    public setVisible(visible: boolean) : void {
        this.sprite.visible = visible;
    }

    public kill() {
        this.sprite.destroy();
        this.sprite = null;
    }

    private updateFrame() {
        if (this.state) {
            if (this.sprite.animations.getAnimation(this.state) != null) {
                this.sprite.animations.play(this.state);
            } else {
                this.sprite.frameName = `objects/${this.id}/${this.state}`;
            }    
        } else {
            this.sprite.frameName = `objects/${this.id}`;
        }
    }
}
