/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"
import { Settings } from "./settings"

export class Cursor {

    private sprite: Phaser.Sprite;

    constructor(private game: Phaser.Game, private layers: Layers) {
    }

    public create() {

        // Create a sprite for the cursor.
        this.sprite = this.game.add.sprite(this.game.world.centerX, this.game.world.centerY, "cursor");
        this.sprite.anchor.set(0.5);

        this.layers.cursor.add(this.sprite);

        let cursorBlink = this.sprite.animations.add("blink");

        this.sprite.animations.play("blink", 6, true);

        // Go to full screen on mousedown.
        this.game.canvas.addEventListener("mousedown", () => {
            
            if (Settings.ENABLE_FULL_SCREEN)
            {
                this.game.scale.startFullScreen(false);
            }
        });

        // Register a move callback to render our own cursor.
        this.game.input.addMoveCallback((pointer: any, x: any, y: any, click: any) => {
            if (!click) {
                this.sprite.x = this.game.input.x;
                this.sprite.y = this.game.input.y;
            }
        }, this);
    }
}