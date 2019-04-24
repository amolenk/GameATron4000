/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Actor } from "./actor"
import { BotClient } from "./botclient"
import { Layers } from "./layers"

export class ConversationUI {

    constructor(private game: Phaser.Game, private botClient: BotClient, private layers: Layers) {
    }

    public displaySuggestedActions(actor: Actor, suggestedActions: any) {
        var options : any[] = [];
        var y = 460;

        // Render suggested actions.
        for (var action of suggestedActions) {

            var optionText = this.game.add.text(10, y, action.displayText, this.createTextStyle());
            optionText.scale.x = 0.5;
            optionText.scale.y = 0.5;
            optionText.fixedToCamera = true;
            optionText.inputEnabled = true;
            optionText.data.value = action.value;

            this.layers.ui.add(optionText);

            optionText.events.onInputOver.add((option: any) => {
                option.addColor("Yellow", 0);
            }, this);

            optionText.events.onInputOut.add((option: any) => {
                option.addColor("", 0);
            }, this);

            optionText.events.onInputUp.add((option: any) => {
                var selectedText = option.text;
                var selectedValue = option.data.value;

                // Destroy text objects used to display options.
                for (var optionToDestroy of options) {
                    optionToDestroy.destroy();
                }

                // TODO await
                actor.sayLine(selectedText).then(
                    () => this.botClient.sendMessageToBot(selectedValue));
            }, this);

            options.push(optionText);
            y += optionText.height;
        }
    }

    private createTextStyle()
    {
        return {
            font: "36px Onesize", // Using a large font-size and scaling it back looks better.
            fill: "#ff0044"
        };
    }
}