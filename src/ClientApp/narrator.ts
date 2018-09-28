/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"
import { Settings } from "./settings"

export class Narrator {

    private text: Phaser.Text;

    constructor(private game: Phaser.Game, private layers: Layers) {
    }

    public create(): void {

        var textStyle = {
            font: "54px Onesize", // Using a large font-size and scaling it back looks better.
            fill: "#0094FF",
            stroke: "black",
            strokeThickness: 12,
            align: "center",
            wordWrap: "true",
            wordWrapWidth: 1400 // Account for scaling.
        };

        this.text = this.game.add.text(400, 150, "", textStyle);
        this.text.anchor.setTo(0.5);
        this.text.lineSpacing = -30;
        this.text.scale.x = 0.5;
        this.text.scale.y = 0.5;

        this.layers.text.add(this.text);
    }

    public async say(text: string) {

        var lines = text.split('\n');
        for (var line of lines) {
            await this.sayLine(line);
        }
    }

    private sayLine(text: string) {

        this.text.setText(text);

        return new Promise((resolve) => {
            
            this.game.time.events.add(
                Math.max(text.length * Settings.TEXT_SPEED, Settings.MIN_TEXT_DURATION),
                () => {
                    this.text.setText('');
                    resolve();
                });
        });
    }
}