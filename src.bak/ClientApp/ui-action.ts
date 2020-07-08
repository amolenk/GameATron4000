/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"

export class ActionUI {

    private text: Phaser.Text;

    constructor(private game: Phaser.Game, private layers: Layers) {
    }

    public create() {

        var textStyle = {
            font: "48px Onesize", // Using a large font-size and scaling it back looks better.
            fill: "white",
            align: "center"
        };

        this.text = this.game.add.text(400, 462, "Walk to", textStyle);
        this.text.anchor.setTo(0.5);
        this.text.lineSpacing = -30;
        this.text.scale.x = 0.5;
        this.text.scale.y = 0.5;
        this.text.fixedToCamera = true;

        this.layers.ui.add(this.text);
    }

    public setText(text: string) {
        this.text.setText(text);
    }

    public setVisible(visible: boolean) {
        this.text.setText("Walk to"); // TODO Default text
        this.text.visible = visible;
    }
}