let scene;
let spriteLookup = [];
let textLookup = [];
let graphics;

function pause() {
    scene.scene.pause();
}

function resume() {
    scene.scene.resume('main');
}

function addImage(x, y, texture, frame) {
    scene.add.image(x, y, texture, frame);
}

function getCameraPosition() {
    return {
        x: scene.cameras.main.scrollX,
        y: scene.cameras.main.scrollY
    }
}

function getPointerPosition() {
    return {
        x: Math.round(scene.input.x + scene.cameras.main.scrollX),
        y: Math.round(scene.input.y + scene.cameras.main.scrollY)
    }
}

function addSprite(spriteKey, textureKey, frameKey, position, origin, depth,
    onPointerDown, onPointerOut, onPointerOver, scrollFactor) {
    const sprite = scene.add.sprite(position.x, position.y, textureKey, frameKey);
    sprite.setOrigin(origin.x, origin.y);
    sprite.setDepth(depth);

    if (scrollFactor !== -1)
    {
        sprite.setScrollFactor(scrollFactor);
    }

    if (onPointerDown || onPointerOut || onPointerOver) {
        sprite.setInteractive({ pixelPerfect: true });
        if (onPointerDown) {
            sprite.on('pointerdown', async function () {
                await onPointerDown.invokeMethodAsync('Invoke', getPointerPosition());
            });
        }
        if (onPointerOut) {
            sprite.on('pointerout', async function () {
                await onPointerOut.invokeMethodAsync('Invoke', getPointerPosition());
            });
        }
        if (onPointerOver) {
            sprite.on('pointerover', async function () {
                await onPointerOver.invokeMethodAsync('Invoke', getPointerPosition());
            });
        }
    }

    spriteLookup[spriteKey] = sprite;

    return {
        width: sprite.width,
        height: sprite.height
    };
}

function destroySprite(spriteKey) {
    spriteLookup[spriteKey].destroy();
    spriteLookup[spriteKey] = null;
}

function addSpriteAnimation(spriteKey, animationKey, atlasKey, spec) {
    spriteLookup[spriteKey].anims.create({
        key: animationKey,
        frames: scene.anims.generateFrameNames(atlasKey, {
            prefix: spec.framePrefix,
            start: spec.frameStart,
            end: spec.frameEnd,
            zeroPad: spec.frameZeroPadding
        }),
        frameRate: spec.frameRate,
        repeat: spec.repeat,
        repeatDelay: spec.repeatDelay
    });
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

function setSpriteDepth(spriteKey, index) {
    spriteLookup[spriteKey].setDepth(index);
}

function moveSprite(spriteKey, x, y, duration, onUpdate, onComplete) {
    var sprite = spriteLookup[spriteKey];
    scene.tweens.add({
        targets: sprite,
        x: x,
        y: y,
        duration: duration,
        onUpdate: function(tween) {
            if (!tween.isCancelled)
            {
                 var continueTween = onUpdate.invokeMethod('Invoke', { x: sprite.x, y: sprite.y });
                 if (!continueTween) {
                    tween.complete();
                    tween.isCancelled = true;
             }
            }
        },
        onComplete: function(tween) {
            if (!tween.isCompleted) {
                onComplete.invokeMethod('Invoke', { x: sprite.x, y: sprite.y });
                tween.isCompleted = true;
            }
        }
    });
}

function addText(textKey, position, text, options) {
    
    var style = {
        font: "24px Onesize",
        fill: options.fillColor,
        stroke: "black",
        strokeThickness: 6,
        align: "center",
    };

    if (options.wordWrapWidth > 0)
    {
        style.wordWrap = {
            width: options.wordWrapWidth
        };
    }
    
    textbox = scene.add.text(position.x, 0, text, style);
    textbox.setOrigin(options.origin.x, options.origin.y);
    textbox.y = position.y - (textbox.height / 2);
    textLookup[textKey] = textbox;
    textbox.setDepth(options.depth);

    if (options.scrollFactor !== -1)
    {
        textbox.setScrollFactor(options.scrollFactor);
    }
}

function destroyText(textKey) {
    textLookup[textKey].destroy();
    textLookup[textKey] = null;
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

function setCameraBounds(size) {
    scene.cameras.main.setBounds(0, 0, size.width, 450); // TODO
}

function startPhaser(container, width, height, dotNetSceneCallback) {

    this.dotNetSceneCallback = dotNetSceneCallback;

    var sceneConfig = {
        key: 'main',
        preload: function () {
            scene = this;
            dotNetSceneCallback.invokeMethod('OnPreload');
        },
        create: function () {
            graphics = this.add.graphics();
            graphics.setDepth(1); // TODO
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
