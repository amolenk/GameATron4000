/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"

declare var options: any;

export class Cursor {

    private sprite: Phaser.Sprite;

    constructor(private game: Phaser.Game, private layers: Layers) {
    }

    public create() {

        // Create a sprite for the cursor.
        this.sprite = this.game.add.sprite(this.game.world.centerX, this.game.world.centerY, "cursor");
        this.sprite.anchor.set(0.5);
        //this.sprite.fixedToCamera = true;

        this.layers.cursor.add(this.sprite);

        let cursorBlink = this.sprite.animations.add("blink");

        this.sprite.animations.play("blink", 6, true);
    }

    public update() {
        this.sprite.position.set(
            this.game.input.x + this.game.camera.x,
            this.game.input.y + this.game.camera.y);
    }
}