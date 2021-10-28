
// TODO Rename to phaser.interop

let scene;
let spriteLookup = [];
let tweenLookup = [];
let graphics;

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

function addTween(id, spriteId, x, y, duration, callback) {
    var sprite = spriteLookup[spriteId];
    var tween = scene.tweens.add({
        targets: sprite,
        x: x,
        y: y,
        duration: duration,
        onUpdate: function() {
            callback.invokeMethod('OnUpdate', { x: sprite.x, y: sprite.y });
        },
        onComplete: function() {
            callback.invokeMethod('OnComplete', { x: sprite.x, y: sprite.y });
        }
    });
    tweenLookup[id] = tween;
}

function stopTween(tweenId) {
    tweenLookup[tweenId].stop();
    tweenLookup[tweenId] = null; // TODO Kill?
}

function drawLines(lines, lineWidth, color) {
    graphics.lineStyle(lineWidth, color, 1);
    for (line of lines) {
        graphics.lineBetween(line.start.x, line.start.y, line.end.x, line.end.y);
    }
}

function loadAtlas(key, textureUrl, atlasUrl) {
    scene.load.atlas(key, textureUrl, atlasUrl);
}

function setSpriteInteraction(spriteId, eventName, callbackObject, callbackName) {
    const sprite = spriteLookup[spriteId];

    sprite.on(eventName, async function (pointer, localX, localY, even) {
        await callbackObject.invokeMethodAsync(callbackName, 
            {
                // TODO GetMousePosition()
                x: Math.round(scene.input.x),
                y: Math.round(scene.input.y)
            });
    });
}

function setWorldBounds(x, y, width, height) {
    scene.physics.world.setBounds(x, y, width, height);
}

function startPhaser(container, width, height, dotNetSceneCallback) {

    this.dotNetSceneCallback = dotNetSceneCallback;

    var sceneConfig = {
        preload: function () {
            scene = this;
            dotNetSceneCallback.invokeMethod('OnPreload');
        },
        create: function () {
            graphics = this.add.graphics();
            graphics.setDepth(100);
            dotNetSceneCallback.invokeMethod('OnCreate');
        },
        update: function () {
            graphics.clear();
            dotNetSceneCallback.invokeMethod(
                'OnUpdate',
                {
                    x: Math.round(this.input.x),
                    y: Math.round(this.input.y)
                });
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
