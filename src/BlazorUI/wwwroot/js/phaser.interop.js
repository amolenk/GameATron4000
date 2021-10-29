
// TODO Rename to phaser.interop

let scene;
let spriteLookup = [];
let tweenLookup = [];
let graphics;

function addImage(x, y, texture, frame) {
    scene.add.image(x, y, texture, frame);
}

function addSprite(spriteKey, textureKey, frameKey, position, options) {
    const sprite = scene.add.sprite(position.x, position.y, textureKey, frameKey);
    spriteLookup[spriteKey] = sprite;

    sprite.setOrigin(options.origin.x, options.origin.y);

    if (options.isInteractive) {
        sprite.setInteractive();
    }

    return {
        width: sprite.width,
        height: sprite.height
    };
}

function addSpriteAnimation(
    spriteKey,
    key,
    atlasKey,
    framePrefix,
    frameStart,
    frameEnd,
    frameZeroPad,
    frameRate,
    repeat,
    repeatDelay) {
    spriteLookup[spriteKey].anims.create({
        key: key,
        frames: scene.anims.generateFrameNames(atlasKey, {
            prefix: framePrefix,
            start: frameStart,
            end: frameEnd,
            zeroPad: frameZeroPad
        }),
        frameRate: frameRate,
        repeat: repeat,
        repeatDelay: repeatDelay
    });

    // this.anims.create({
    //     key: "fly",
    //     frameRate: 7,
    //     frames: this.anims.generateFrameNames("plane", {
    //         prefix: "plane",
    //         suffix: ".png",
    //         start: 1,
    //         end: 3,
    //         zeroPad: 1
    //     }),
    //     repeat: -1
    // });
    
    //  This code should be run from within a Scene:
    //this.anims.create(config););
}

function playSpriteAnimation(spriteKey, animationKey) {
    spriteLookup[spriteKey].play(animationKey);
}

function stopSpriteAnimation(spriteKey) {
    spriteLookup[spriteKey].stop();
}

function setSpriteFrame(spriteKey, frameName) {
    spriteLookup[spriteKey].setFrame(frameName);
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

function startCameraFollow(spriteKey) {
    scene.cameras.main.startFollow(spriteLookup[spriteKey], true, 0.1, 0.1);
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

    sprite.on(eventName, function (pointer, localX, localY, even) {
        callbackObject.invokeMethod(
            callbackName, 
            {
                // TODO GetMousePosition()
                x: Math.round(scene.input.x + scene.cameras.main.scrollX),
                y: Math.round(scene.input.y + scene.cameras.main.scrollY)
            });
    });
}

// TODO Rename to setCameraBounds
function setWorldBounds(size) {
    scene.cameras.main.setBounds(0, 0, size.width, 450);
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
                    x: Math.round(this.input.x + this.cameras.main.scrollX),
                    y: Math.round(this.input.y + this.cameras.main.scrollY)
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
