/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { Layers } from "./layers"
import { UIMediator } from "./ui-mediator"

declare var gameInfo: any;

export class Actor {

    private WALK_SPEED_FACTOR = 4;

    private game: Phaser.Game;

    private originX: number;
    private originY: number;

    private text: Phaser.Text;
    private walkSprite: Phaser.Sprite;
    private spriteGroup: Phaser.Group;

    private talkAnimation: Phaser.Animation;
    private walkAnimation: Phaser.Animation;
    private backSprite: Phaser.Sprite;

    private isMoving: boolean;
    private moveTween: Phaser.Tween;

    private sprite2: Phaser.Sprite;
    
    public constructor(public id: string, public name: string, public classes: string[], private textColor: string, private direction: string) {
    }

    get spriteDebug() {
        return this.sprite2;
    }

    public get x() {
        return this.sprite2.x;
    }

    public get y() {
        return this.sprite2.y;
    }

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, layers: Layers) {
        
        this.game = game;
        this.originX = x;
        this.originY = y;

        this.sprite2 = this.game.add.sprite(x, y, "sprites", "actors/" + this.id + "/front");
        this.sprite2.anchor.set(0.5, 1);

        // Animations
        this.sprite2.animations.add('walk-left',
            Phaser.Animation.generateFrameNames("actors/" + this.id + "/walk/left/", 1, 6, '', 4), 6, true, false);

        this.sprite2.animations.add('walk-right',
            Phaser.Animation.generateFrameNames("actors/" + this.id + "/walk/right/", 1, 6, '', 4), 6, true, false);

        this.text = this.game.add.text(x, y - this.sprite2.height - 40, "", this.createTextStyle());
        this.text.anchor.setTo(0.5);
        this.text.lineSpacing = -30;
        this.text.scale.x = 0.5;
        this.text.scale.y = 0.5; 

        this.sprite2.addChild(this.text);

        this.sprite2.data.z = y;

        this.spriteGroup = game.add.group();
        this.spriteGroup.addMultiple([ this.sprite2, this.text ]);

        layers.objects.add(this.sprite2);
        layers.text.add(this.text);

        if (this.classes.indexOf("class_untouchable") == -1) {
            this.sprite2.inputEnabled = true;
            this.sprite2.input.pixelPerfectClick = true;
            this.sprite2.input.pixelPerfectOver = true;

            this.sprite2.events.onInputOver.add(() => uiMediator.focusObject(this));
            this.sprite2.events.onInputOut.add(() => uiMediator.focusObject(null));
            this.sprite2.events.onInputDown.add(() => uiMediator.selectObject(this));
        }
    }

    public moveTo(path: Phaser.Point[]): Promise<void> {

        if (this.isMoving) {
            this.moveTween.stop();
        }

        var i = 0;

        return new Promise((resolve) => {

            var move = () => {

                this.sprite2.animations.stop();

                const animation = (this.sprite2.x < path[i].x) ? "walk-right" : "walk-left";
                this.sprite2.animations.play(animation);

                const tweenDuration = Phaser.Math.distance(this.sprite2.x, this.sprite2.y, path[i].x, path[i].y)
                    * this.WALK_SPEED_FACTOR;

                this.isMoving = true;
                this.moveTween = this.game.add.tween(this.sprite2).to({ x: path[i].x, y: path[i].y }, tweenDuration).start();
        
                this.moveTween.onUpdateCallback(() => {
                    this.sprite2.scale.set(this.sprite2.y / 400)
                    this.sprite2.data.z = this.sprite2.y;
                });
                this.moveTween.onComplete.add(() => {
                    i += 1;
                    if (i < path.length) {
                        move();
                    } else {
                        this.isMoving = false;
                        this.sprite2.animations.stop();
                        this.sprite2.frameName = "actors/" + this.id + "/front";
                        resolve();
                    }
                });
            };

            move();
        });
    }

    public async say(text: string) {
        
        var lines = text.split('\n');
        for (var line of lines) {
            await this.sayLine(line);
        }
    }

    public async changeDirection(direction: string) {
        // TODO
        // if (direction == "Front") {
        //     this.backSprite.visible = false;
        //     this.sprite.visible = true;
        // } else if (direction == "Away") {
        //     this.sprite.visible = false;
        //     this.backSprite.visible = true;
        // }
        // this.direction = direction;
        return Promise.resolve();
    }

    public update() {
        this.text.x = Math.floor(this.sprite2.x);
        this.text.y = Math.floor(this.sprite2.y - this.sprite2.height - 40);
    }

    public kill() {

        this.sprite2.destroy();
        this.text.destroy();
    }

    private sayLine(text: string) {

        // if (!this.backSprite.visible) {
        //     this.talkAnimation.play(6, true);
        // }
        this.text.setText(text);

        return new Promise((resolve) => {
            
            this.game.time.events.add(
                Math.max(text.length * gameInfo.textSpeed, gameInfo.minTextDuration),
                () => {
                    this.text.setText('');
                    // if (!this.backSprite.visible) {
                    //     this.talkAnimation.stop(true);
                    // }
                    resolve();
                });
        });
    }

    private createTextStyle() {

        return {
            font: "54px Onesize", // Using a large font-size and scaling it back looks better.
            fill: this.textColor,
            stroke: "black",
            strokeThickness: 12,
            align: "center",
            wordWrap: "true",
            wordWrapWidth: 600 // Account for scaling.
        };
    }
}
