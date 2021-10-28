/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"

declare var gameInfo: any;

// TODO Clean-up after textbox updates
export class Actor {

    private WALK_SPEED_FACTOR = 4;

    private textBox: Phaser.Text;
    private sprite: Phaser.Sprite;

    private moveTween: Phaser.Tween;

    private game: Phaser.Game;
    private layers: Layers;
    private scale: any;

    private spaceKey: Phaser.Key;

    private resolveWaitForLine: any;

    public constructor(
        public id: string,
        public name: string,
        public classes: string[],
        public usePosition: string,
        public useDirection: string,
        private faceDirection: string,
        private talkColor: string) {
    }

    public get x() {
        return this.sprite.x;
    }

    public get y() {
        return this.sprite.y;
    }

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, layers: Layers,
        scale: any) {
        
        this.game = game;
        this.layers = layers;
        this.scale = scale;

        // TODO Refactor to GetFrameName
        var frameName = (this.classes.indexOf("class_invisible") == -1)
            ? `actors/${this.id}/${this.faceDirection}` : `./transparent`;

        this.sprite = this.game.add.sprite(x, y, "sprites", frameName);
        this.sprite.anchor.set(0.5, 1);
        this.sprite.data.z = y;

        // Animations
        if (this.classes.indexOf("class_invisible") == -1) {
            this.sprite.animations.add('walk-left',
                Phaser.Animation.generateFrameNames(`actors/${this.id}/walk/left/`, 1, 6, '', 4), 9, true, false);

            this.sprite.animations.add('walk-right',
                Phaser.Animation.generateFrameNames(`actors/${this.id}/walk/right/`, 1, 6, '', 4), 9, true, false);

            this.sprite.animations.add('talk',
                Phaser.Animation.generateFrameNames(`actors/${this.id}/talk/`, 1, 6, '', 4), 9, true, false);
        }

        // this.text = this.game.add.text(x, y - this.sprite.height - 40, "", this.createTextStyle(0));
        // this.text.anchor.setTo(0.5);
        // this.text.lineSpacing = -30;
        // this.text.scale.x = 0.5;
        // this.text.scale.y = 0.5; 

        if (this.classes.indexOf("class_untouchable") == -1) {
            this.sprite.inputEnabled = true;
            this.sprite.input.pixelPerfectClick = true;
            this.sprite.input.pixelPerfectOver = true;

            this.sprite.events.onInputOver.add(() => uiMediator.focusObject(this));
            this.sprite.events.onInputOut.add(() => uiMediator.focusObject(null));
            this.sprite.events.onInputDown.add(() => uiMediator.selectObject(this));
        }

        this.updateScale();
        this.sprite.data.z = this.sprite.y;

        layers.objects.add(this.sprite);
//        layers.text.add(this.text);

        this.spaceKey = game.input.keyboard.addKey(Phaser.Keyboard.SPACEBAR);

        this.spaceKey.onDown.add(
            () => {
                if (this.resolveWaitForLine) {
                    this.resolveWaitForLine();
                    this.resolveWaitForLine = null;
                }
            });
    }

    public changeDirection(direction: string) {
        this.sprite.frameName = `actors/${this.id}/${direction}`;
        this.faceDirection = direction;
    }

    public focusCamera(jump: boolean) {
        if (jump) {
            this.game.camera.focusOn(this.sprite);
        }
        this.game.camera.follow(this.sprite, Phaser.Camera.FOLLOW_LOCKON, 0.1, 0.1);
    }

    public async sayLine(text: string) {

        // TODO Constant
        if (this.faceDirection == "front") {
            this.sprite.animations.play("talk");
        }

        const spaceLeft = this.sprite.x - this.game.camera.x;
        const spaceRight = this.game.camera.x + this.game.camera.width - this.sprite.x;

        const maxWordWrap = Math.min(spaceLeft, spaceRight) * 2;

        this.textBox = this.game.add.text(this.sprite.x, 0, text, this.createTextStyle(maxWordWrap));
        this.textBox.anchor.setTo(0.5);
        this.textBox.lineSpacing = -30;
        this.textBox.scale.x = 0.5;
        this.textBox.scale.y = 0.5; 

        this.textBox.y = this.sprite.y - this.sprite.height - (this.textBox.height / 2);


        this.layers.text.add(this.textBox);

    //  this.text.setText(text);

        await this.waitWhileSpeaking(text.length);

        // return new Promise((resolve) => {
            
        //     this.game.time.events.add(
        //         Math.max(text.length * gameInfo.textSpeed, gameInfo.minTextDuration),
        //         () => {
                    this.layers.text.remove(this.textBox, true);
//                    this.textBox.kill();
//                    this.text.setText('');
                    if (this.faceDirection == "front") {
                        this.sprite.animations.stop("talk", true);

                        // Reset the frame.
                        this.changeDirection(this.faceDirection);
                    }
        //             resolve();
        //         });
        // });
    }

    private waitWhileSpeaking(textLength: number) {
        return new Promise((resolve) => {
            
            this.resolveWaitForLine = resolve;

            this.game.time.events.add(
                Math.max(textLength * gameInfo.textSpeed, gameInfo.minTextDuration),
                () => {
                    if (this.resolveWaitForLine) {
                        this.resolveWaitForLine();
                        this.resolveWaitForLine = null;
                    }
                });

        });
    }

    public walkTo(path: Phaser.Point[], faceDirection: string): Promise<void> {

        if (this.moveTween) {
            this.moveTween.stop();
        }

        var i = 0;

        return new Promise((resolve) => {

            var move = () => {

                this.sprite.animations.stop();

                const animation = (this.sprite.x < path[i].x) ? "walk-right" : "walk-left";
                this.sprite.animations.play(animation);

                const tweenDuration = Phaser.Math.distance(this.sprite.x, this.sprite.y, path[i].x, path[i].y)
                    * this.WALK_SPEED_FACTOR;

                this.moveTween = this.game.add.tween(this.sprite).to({ x: path[i].x, y: path[i].y }, tweenDuration).start();
        
                this.moveTween.onUpdateCallback(() => {
                    this.updateScale();
                    this.sprite.data.z = this.sprite.y;
                });
                this.moveTween.onComplete.add(() => {
                    i += 1;
                    if (i < path.length) {
                        move();
                    } else {
                        this.moveTween = null;
                        this.sprite.animations.stop();
                        this.changeDirection(faceDirection);
                        resolve();
                    }
                });
            };

            move();
        });
    }

    public update() {
//        this.text.x = Math.floor(this.sprite.x);
  //      this.text.y = Math.floor(this.sprite.y - this.sprite.height - 40);
    }

    public kill() {
        this.sprite.destroy();
    //    this.text.destroy();
    }

    private updateScale() {
        if (this.scale) {
            if (this.sprite.y < this.scale.start) {
                this.sprite.scale.set(this.scale.min / 100);
            }
            else if (this.sprite.y > this.scale.start && this.sprite.y < this.scale.end) {
                const scaleAreaHeight = this.scale.end - this.scale.start;
                const scaleDiff = this.scale.max - this.scale.min;
                const percentageInArea = (this.sprite.y - this.scale.start) / scaleAreaHeight;
                const scale = scaleDiff * percentageInArea + this.scale.min;
                this.sprite.scale.set(scale / 100);
            }
            else {
                this.sprite.scale.set(this.scale.max / 100);
            }
        }
        else {
            this.sprite.scale.set(1);
        }
    }

    // TODO Extract
    private createTextStyle(maxWordWrap: number) {

        return {
            font: "54px Onesize", // Using a large font-size and scaling it back looks better.
            fill: this.talkColor,
            stroke: "black",
            strokeThickness: 12,
            align: "center",
            wordWrap: "true",
            wordWrapWidth: Math.min(600, maxWordWrap) * 2 // Account for scaling.
        };
    }
}