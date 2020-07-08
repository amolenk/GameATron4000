/// <reference path="../node_modules/phaser/typescript/phaser.d.ts" />

declare var gameInfo: any;

export class Assets {

    public static preload(game: Phaser.Game) {

        for (let asset of gameInfo.assets) {
            game.load.atlasJSONHash(asset, "/dist/gameplay/" + asset + ".png", "/dist/gameplay/" + asset + ".json");
        }
    }
}