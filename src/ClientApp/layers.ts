/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

export class Layers {

    private backgroundGroup: Phaser.Group;
    private objectGroup: Phaser.Group;
    private actorGroup: Phaser.Group;
    private textGroup: Phaser.Group;
    private uiGroup: Phaser.Group;
    private cursorGroup: Phaser.Group;

    constructor(private game: Phaser.Game) {
    }

    get background(): Phaser.Group {
        return this.backgroundGroup;
    }

    get objects(): Phaser.Group {
        return this.objectGroup;
    }

    get actors(): Phaser.Group {
        return this.actorGroup;
    }

    get text(): Phaser.Group {
        return this.textGroup;
    }

    get ui(): Phaser.Group {
        return this.uiGroup;
    }

    get cursor(): Phaser.Group {
        return this.cursorGroup;
    }

    public create() {

        this.backgroundGroup = this.game.add.group();
        this.objectGroup = this.game.add.group();
        this.actorGroup = this.game.add.group();
        this.textGroup = this.game.add.group();
        this.uiGroup = this.game.add.group();
        this.cursorGroup = this.game.add.group();
    }
}