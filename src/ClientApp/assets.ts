/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

declare var GameAssets: any;

export class Assets {

    public static preload(game: Phaser.Game) {

        for (let asset of GameAssets) {

            if (asset.frameWidth && asset.frameHeight) {
                game.load.spritesheet(asset.key, asset.url, asset.frameWidth, asset.frameHeight);
            } else {
                game.load.image(asset.key, asset.url);
            }
        }
    }
}
