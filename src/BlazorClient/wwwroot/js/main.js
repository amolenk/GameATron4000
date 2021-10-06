let scene;
let spriteLookup = [];

function addImage(x, y, key) {
    scene.add.image(x, y, key);
}

function addImage(x, y, texture, frame) {
    scene.add.image(x, y, texture, frame);
}

function addSprite(x, y, key) {
    const sprite = scene.add.sprite(x, y, key);

    const spriteKey = `${sceneKey}-${key}`;
    spriteLookup[spriteKey] = sprite;

    return spriteKey;
}

function addText(x, y, text) {
    scene.add.text(x, y, text, { fontFamily: 'Helvetica' });
}

function loadAtlas(key, textureUrl, atlasUrl) {
    scene.load.atlas(key, textureUrl, atlasUrl);
}

function loadImage(key, imageUrl) {
    scene.load.image(key, imageUrl);
}

function setSpriteInteraction(spriteKey, callbackObject, eventName) {
    const sprite = spriteLookup[spriteKey];

    console.log('registering!: ' + eventName);

    sprite.setInteractive().on(eventName, async function (pointer, localX, localY, even) {

        console.log(eventName + ' callback started!');

        await callbackObject.invokeMethodAsync('OnInteractionAsync', eventName);

        console.log(eventName + ' callback ended!');
    });
}

function startPhaser(container, dotNetSceneCallback) {

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
        type: Phaser.CANVAS,
        width: 640,//1280,
        height: 480,//600,
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
