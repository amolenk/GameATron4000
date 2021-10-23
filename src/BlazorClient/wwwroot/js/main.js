
// TODO Rename to phaser.interop

let scene;
let spriteLookup = [];

function addImage(x, y, texture, frame) {
    scene.add.image(x, y, texture, frame);
}

function addSprite(spriteId, x, y, texture, frame, options) {
    const sprite = scene.add.sprite(x, y, texture, frame, options);
    spriteLookup[spriteId] = sprite;

    sprite.setOrigin(options.originX, options.originY);

    if (options.isInteractive) {
        sprite.setInteractive();
    }

    return {
        id: spriteId,
        width: sprite.width,
        height: sprite.height
    };
}

function addText(x, y, text) {
    scene.add.text(x, y, text, { fontFamily: 'Helvetica' });
}

function loadAtlas(key, textureUrl, atlasUrl) {
    scene.load.atlas(key, textureUrl, atlasUrl);
}

function setSpriteInteraction(spriteId, eventName, callbackObject, callbackName) {
    const sprite = spriteLookup[spriteId];

    sprite.on(eventName, async function (pointer, localX, localY, even) {
        await callbackObject.invokeMethodAsync(callbackName);
    });
}

function setWorldBounds(x, y, width, height) {
    scene.physics.world.setBounds(x, y, width, height);
}

function startPhaser(container, width, height, dotNetSceneCallback) {

    this.dotNetSceneCallback = dotNetSceneCallback;

    var sceneConfig = {
        preload: async function () {
            scene = this;
            await dotNetSceneCallback.invokeMethodAsync('PreloadAsync', key);
        },
        create: async function () {
            await dotNetSceneCallback.invokeMethodAsync('CreateAsync', key);
        },
        update: async function () {
            await dotNetSceneCallback.invokeMethodAsync('UpdateAsync', key);
        }
    };

    var gameConfig = {
        title: 'Game-a-Tron 4000™',
        type: Phaser.AUTO,
        width: width,
        height: height,
        backgroundColor: '#336023',
        parent: container,
        pixelArt: true,
        physics: {
            default: 'arcade',
            arcade: { debug: false }
        },
        scale: {
            mode: Phaser.Scale.FIT,
            autoCenter: Phaser.Scale.CENTER_HORIZONTALLY
        },
        scene: sceneConfig
    };

    new Phaser.Game(gameConfig);
}
