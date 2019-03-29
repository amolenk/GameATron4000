/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import './styles/site.css'
const CursorImage = require('./assets/cursor.png')
const VerbsImage = require('./assets/verbs.png')
const VerbsImageData = require('./assets/verbs.json')

import { Assets } from "./assets"
import { BotClient } from "./botclient"
import { Cursor } from "./cursor"
import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"

class GameATron {

    private game: Phaser.Game;
    private cursor: Cursor;

    private layers: Layers;
    private uiMediator: UIMediator;

    private debugMode: boolean;

    constructor() {

        console.log("Welcome to Game-a-Tron 4000 🤖")

        this.game = new Phaser.Game(
            800,
            600,
            Phaser.AUTO, // Default renderer
            "content", // DOM element
            this,
            false, // transparent
            false); // anti-aliasing

        this.layers = new Layers(this.game);
        this.cursor = new Cursor(this.game, this.layers);
        this.uiMediator = new UIMediator(this.game, this.cursor, new BotClient(), this.layers);
    }

    private preload() {

       this.game.load.spritesheet("cursor", CursorImage, 58, 58);
       this.game.load.atlas("verbs", VerbsImage, null, VerbsImageData);

       Assets.preload(this.game);
    }

    private create() {

        this.game.scale.fullScreenScaleMode = Phaser.ScaleManager.SHOW_ALL;
        this.game.stage.smoothed = true;

        this.layers.create();
        this.cursor.create();
        this.uiMediator.create();

        var debugKey = this.game.input.keyboard.addKey(Phaser.Keyboard.D);
        debugKey.onDown.add(() => {
            this.debugMode = !this.debugMode;
            this.game.debug.reset();
        });

        var fullscreenKey = this.game.input.keyboard.addKey(Phaser.Keyboard.F);
        fullscreenKey.onDown.add(() => {
            if (this.game.scale.isFullScreen) {
                this.game.scale.stopFullScreen();
            } else {
                this.game.scale.startFullScreen(false);
            }
        });
    }

    private update() {

        // Depth sorting based on y-axis position.
        this.layers.objects.sort('y', Phaser.Group.SORT_ASCENDING);

        this.uiMediator.update();

        // Draw the cursor in the update loop so the sprite is also updated
        // when the camera moves.
        this.cursor.update();
    }

    private render() {
        if (this.debugMode) {
            this.uiMediator.debug();
            this.game.debug.inputInfo(32, 32);
        }
    }
}

window.onload = () => {
    new GameATron();
};

if (module.hot) {
    module.hot.accept();
}