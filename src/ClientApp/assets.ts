/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

declare var GameAssets: any;

export class Assets {

    public static preload(game: Phaser.Game) {

        for (let asset of GameAssets) {

            if (asset.FrameWidth && asset.FrameHeight) {
                game.load.spritesheet(asset.Key, asset.Url, asset.FrameWidth, asset.FrameHeight);
            } else {
                game.load.image(asset.Key, asset.Url);
            }
        }
    }
}
