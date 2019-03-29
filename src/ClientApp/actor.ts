/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

import { RoomObject } from "./room-object"
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
    
    public constructor(public name: string, public displayName: string, private textColor: string, private direction: string) {

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

    public create(game: Phaser.Game, uiMediator: UIMediator, x: number, y: number, group: Phaser.Group) {
        
        //super.create(game, uiMediator, x, y, group);

        this.game = game;
        this.originX = x;
        this.originY = y;

        // Actors are anchored at the bottom instead of middle for easier placement.
        // TODO not a field??
// game.add.sprite
        this.sprite2 = this.game.add.sprite(x, y, "sprites", "actors/" + this.name + "/front");
        this.sprite2.anchor.set(0.5, 1);

        // Animations
        this.sprite2.animations.add('walk-left',
            Phaser.Animation.generateFrameNames("actors/" + this.name + "/walk/left/", 1, 6, '', 4), 6, true, false);

        this.sprite2.animations.add('walk-right',
            Phaser.Animation.generateFrameNames("actors/" + this.name + "/walk/right/", 1, 6, '', 4), 6, true, false);

//        this.sprite2.animations.play("actor-guy-walk");


        // this.walkSprite = game.add.sprite(x, y, this.name + "-walk");
        // this.walkSprite.anchor.set(0.5, 1);
        // this.walkSprite.inputEnabled = true;
        // this.walkSprite.input.pixelPerfectClick = true;
        // this.walkSprite.input.pixelPerfectOver = true;
        // this.walkSprite.visible = false;

        // this.backSprite = game.add.sprite(x, y, this.name + "-back");
        // this.backSprite.anchor.set(0.5, 1);
        // this.backSprite.visible = false;

        this.text = this.game.add.text(x, y - this.sprite2.height - 40, "", this.createTextStyle());
        this.text.anchor.setTo(0.5);
        this.text.lineSpacing = -30;
        this.text.scale.x = 0.5;
        this.text.scale.y = 0.5;

//        this.sprite2.addChild(this.text);

        // this.talkAnimation = this.sprite.animations.add("talk");
        // this.walkAnimation = this.walkSprite.animations.add("walk");

        this.sprite2.addChild(this.text);


        this.spriteGroup = game.add.group();
        this.spriteGroup.addMultiple([ this.sprite2, this.text ]);

        group.add(this.sprite2);
        group.add(this.text);

        // if (this.direction == "Front") {
        //     this.backSprite.visible = false;
        //     this.sprite.visible = true;
        // } else if (this.direction == "Away") {
        //     this.sprite.visible = false;
        //     this.backSprite.visible = true;
        // }

        // group.add(this.spriteGroup);
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
        
                this.moveTween.onUpdateCallback(() => this.sprite2.scale.set(this.sprite2.y / 400));
                this.moveTween.onComplete.add(() => {
                    i += 1;
                    if (i < path.length) {
                        move();
                    } else {
                        this.isMoving = false;
                        this.sprite2.animations.stop();
                        this.sprite2.frameName = "actors/" + this.name + "/front";
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

    public async walkTo(x: number, y: number): Promise<void> {

        // Ensure that we're facing front.
        if (this.direction != 'Front') {
            this.backSprite.visible = false;
            this.sprite2.visible = true;
        }

        // Calculate the delta's compared to the original position.
        var deltaX = x - this.originX;
        var deltaY = y - this.originY;

        // No need to walk anywhere if the actor's already there.
        if (deltaX == this.spriteGroup.x && deltaY == this.spriteGroup.y) {
            return;
        }

        // Walk animation plays at 6 frames per second.
        // A single loop of the walk animation covers 100 pixels.
        // So, an actor walks 100 pixels per second.
        var duration = Math.abs((deltaX - this.spriteGroup.x) / 100 * 1000); // Milliseconds.

        // If the actor walks to the left, flip the walk sprite.
        // Otherwise, reset the sprite if it is already flipped.
        var rightToLeft = deltaX < this.spriteGroup.x;
        var isFlipped = this.walkSprite.scale.x < 0;

        if ((rightToLeft && !isFlipped) || (!rightToLeft && isFlipped)) {
            this.walkSprite.scale.x *= -1;
        }

        // Switch to the walk sprite.
        this.walkAnimation.play(6, true);
        this.sprite2.visible = false;
        this.walkSprite.visible = true;
                
        // Animate!
        var tween = this.game.add.tween(this.spriteGroup)
            .to( { x: deltaX, y: deltaY }, duration, Phaser.Easing.Default, true);

        await new Promise((resolve) => {
            tween.onComplete.add(() => {

                this.walkAnimation.stop();

                // Switch back to the correct sprite.
                this.walkSprite.visible = false;
                if (this.direction == "Front") {
                    this.sprite2.visible = true;
                } else if (this.direction == "Away") {
                    this.backSprite.visible = true;
                }

                resolve();
            });
        });
    }

    public update() {
        this.text.x = Math.floor(this.sprite2.x);
        this.text.y = Math.floor(this.sprite2.y - this.sprite2.height - 40);
    }

    public kill() {

        this.walkSprite.destroy();
        this.backSprite.destroy();
        this.text.destroy();
        this.talkAnimation.destroy();
        this.walkAnimation.destroy();

//        super.kill();
    }

    private sayLine(text: string) {

        console.log(text);

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
