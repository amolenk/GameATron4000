const sceneInfos = [];
let currentScene;

// TODO Extract more generic method
function findSprite(sceneName, spriteName) {
    const sceneInfo = sceneInfos[sceneName];
    return sceneInfo.phaserScene.children.list.find(child => {
        return child.type === "Sprite" && child.name === spriteName
    });
}

function findText(sceneName, textName) {
    const sceneInfo = sceneInfos[sceneName];
    return sceneInfo.phaserScene.children.list.find(child => {
        return child.type === "Text" && child.name === textName
    });
}

function addSpriteEventHandler(sceneName, spriteName, eventName, handlerName, async) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setInteractive({ pixelPerfect: true });
    sprite.on(eventName, (pointer) => {
        const sceneInfo = sceneInfos[sceneName];
        const eventArgs = { 'spriteName': spriteName, 'spriteX': sprite.x, 'spriteY': sprite.y, 'x': pointer.x, 'y': pointer.y, 'distance': -1 };
        if (pointer.distance) eventArgs.distance = pointer.distance;
        if (async)
            sceneInfo.dotNetScene.invokeMethodAsync(handlerName, eventArgs);
        else
            sceneInfo.dotNetScene.invokeMethod(handlerName, eventArgs);
    });
}

function addSceneEventHandler(sceneName, eventName, handlerName) {
    const sceneInfo = sceneInfos[sceneName];
    sceneInfo.phaserScene.input.on(eventName, (pointer) => {
        const eventArgs = { 'x': pointer.x, 'y': pointer.y, 'distance': -1 };
        sceneInfo.dotNetScene.invokeMethod(handlerName, eventArgs);
    });
}

function setSpriteCrop(sceneName, spriteName, x, y, width, height) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setCrop(x, y, width, height);
}

function setSpriteScale(sceneName, spriteName, scale) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.scale = scale;
}

function setSpriteLocation(sceneName, spriteName, x, y) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.x = x;
    sprite.y = y;
}

function setSpriteTint(sceneName, spriteName, color) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setTint(color);
}

function spriteExists(sceneName, spriteName) {
    return findSprite(sceneName, spriteName) != null;
}

function setTextValue(sceneName, textName, value) {
    const text = findText(sceneName, textName);
    text.setText(value);
}

function setTextOrigin(sceneName, textName, originX, originY) {
    const text = findText(sceneName, textName);
    text.setOrigin(originX, originY);
}

function getSpriteData(sceneName, spriteName, key) {
    const sprite = findSprite(sceneName, spriteName);
    return sprite.getData(key);
}

function setSpriteData(sceneName, spriteName, key, value) {
    const sprite = findSprite(sceneName, spriteName);
    sprite.setData(key, value);
}

function addSprite(sceneName, spriteName, imageName, x, y, scale = null, interactive = false) {
    const sceneInfo = sceneInfos[sceneName];
    const sprite = sceneInfo.phaserScene.add.sprite(x, y, 'sprites');
    sprite.name = spriteName;
    sprite.setFrame(imageName);
    if (scale) {
        sprite.setScale(scale);
    }
}

function addText(sceneName, textName, x, y, value, fontSize, fill, style) {
    const sceneInfo = sceneInfos[sceneName];
    const text = sceneInfo.phaserScene.add.text(x, y, value, { fontSize: fontSize, fill: fill });
    text.name = textName;
    text.setFontFamily('Arial');
    text.setOrigin(0, 0.5);

    if (style) {
        text.setFontStyle(style);
    }
}

function addFireworks(sceneName) {
    const sceneInfo = sceneInfos[sceneName];

    var p0 = new Phaser.Math.Vector2(300, 1024);
    var p1 = new Phaser.Math.Vector2(300, 750);
    var p2 = new Phaser.Math.Vector2(980, 750);
    var p3 = new Phaser.Math.Vector2(980, 1024);

    var curve = new Phaser.Curves.CubicBezier(p0, p1, p2, p3);

    var max = 28;
    var points = [];
    var tangents = [];

    for (var c = 0; c <= max; c++)
    {
        var t = curve.getUtoTmapping(c / max);

        points.push(curve.getPoint(t));
        tangents.push(curve.getTangent(t));
    }

    var tempVec = new Phaser.Math.Vector2();

    var spark0 = sceneInfo.phaserScene.add.particles('particle-white');
    var spark1 = sceneInfo.phaserScene.add.particles('particle-yellow');

    var emitters = [];

    for (var i = 0; i < points.length; i++)
    {
        var p = points[i];

        tempVec.copy(tangents[i]).normalizeRightHand().scale(-32).add(p);

        var angle = Phaser.Math.RadToDeg(Phaser.Math.Angle.BetweenPoints(p, tempVec));

        var particles = (i % 2 === 0) ? spark0 : spark1;

        emitters.push(particles.createEmitter({
            x: tempVec.x,
            y: tempVec.y,
            angle: angle,
            speed: { min: -100, max: 500 },
            gravityY: 200,
            scale: { start: 0.4, end: 0.1 },
            lifespan: 800,
            blendMode: 'SCREEN',
            on: true
        }));
    }

    sceneInfo.phaserScene.time.delayedCall(
        250,
        (emittersToStop) => {
            for (let i = 0; i < emittersToStop.length; i++) {
                emittersToStop[i].on = false;
            }
        },
        [emitters]);
}

function removeSprite(sceneName, spriteName) {
    const sprite = findSprite(sceneName, spriteName);
    if (sprite) {
        sprite.destroy();
    }
}

function isSceneVisible(scene) {
    return game.scene.isActive(scene)
        || sceneInfos[scene].isCreating;
}

function switchScene(from, to) {
    stopScene(from);
    startScene(to);
}

function startScene(scene) {
    game.scene.start(scene);
}

function stopScene(scene) {
    game.scene.stop(scene);
}

function shakeCamera(scene) {
    sceneInfos[scene].phaserScene.cameras.main.flash(250);
}

function registerScene(name, dotNetScene) {

    var phaserScene = new Phaser.Scene(name);
    sceneInfos[name] = {
        phaserScene: phaserScene,
        dotNetScene: dotNetScene
    };

    // TODO Use pack instead of hard coded: https://phaser.io/examples/v3/view/scenes/swapping-scenes
    phaserScene.preload = function () {
        this.load.image('particle-white', './assets/white.png');
        this.load.image('particle-yellow', './assets/yellow.png');
        this.load.atlas("sprites", './assets/sprites.png', './assets/sprites.json');
    };

    phaserScene.create = function () {

        //this.cameras.add(0, 0, 800, 600);
        sceneInfos[name].isCreating = true;
        dotNetScene.invokeMethod('create');
        sceneInfos[name].isCreating = false;
    }
}

let game;

function startPhaser(container, title) {

    const config = {
        type: Phaser.AUTO,
        scale: {
            parent: container,
            width: 1280,
            height: 1024
        },
        scene: Object.keys(sceneInfos).map(key => sceneInfos[key].phaserScene),
        title: title
      };
      
      game = new Phaser.Game(config);
}

