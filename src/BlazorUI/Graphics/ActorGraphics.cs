namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public sealed class ActorGraphics : IDisposable
{
    private const int WALK_SPEED_FACTOR = 4;
    private const int MIN_WORD_WRAP_WIDTH = 400;

    private const int DELAY_PER_CHAR = 75;
    private const int MIN_TEXT_DELAY = 1500;

    private const string ANIM_WALK_LEFT = "walk-left";
    private const string ANIM_WALK_RIGHT = "walk-right";
    private const string ANIM_TALK = "talk";

    private readonly IGraphics _graphics;

    public Actor Actor { get; }
    public ISprite Sprite { get; }

    private ActorGraphics(Actor actor, ISprite sprite, IGraphics graphics)
    {
        _graphics = graphics;

        Actor = actor;
        Sprite = sprite;
    }

    public static ActorGraphics Create(
        Actor actor,
        Func<GameObject, Point, Task> onPointerDown,
        Func<GameObject, Point, Task> onPointerOut,
        Func<GameObject, Point, Task> onPointerOver,
        IGraphics graphics)
    {
        var sprite = graphics.AddSprite(
            "images", // TODO
            GetFrameName(actor, Direction.Front),
            actor.Position,
            options =>
            {
                options.Origin = new Point(0.5, 1);
                options.Depth = actor.Position.Y;

                if (actor.IsTouchable)
                {
                    options.OnPointerDown = (pointerPosition) =>
                        onPointerDown(actor, pointerPosition);
                    options.OnPointerOut = (pointerPosition) =>
                        onPointerOut(actor, pointerPosition);
                    options.OnPointerOver = (pointerPosition) =>
                        onPointerOver(actor, pointerPosition);
                }
            });

        // Animations
        // TODO Make global and register from script!
        if (actor.IsVisible)
        {
            sprite.AddAnimation(
                ANIM_WALK_LEFT,
                framePrefix: $"actors/{actor.Id}/walk/left/",
                frameStart: 1,
                frameEnd: 6,
                frameZeroPad: 4,
                frameRate: 9);
            
            sprite.AddAnimation(
                ANIM_WALK_RIGHT,
                framePrefix: $"actors/{actor.Id}/walk/right/",
                frameStart: 1,
                frameEnd: 6,
                frameZeroPad: 4,
                frameRate: 9);

            sprite.AddAnimation(
                ANIM_TALK,
                framePrefix: $"actors/{actor.Id}/talk/",
                frameStart: 1,
                frameEnd: 4,
                frameZeroPad: 4,
                frameRate: 9);
        }

        return new ActorGraphics(actor, sprite, graphics);
    }

    public async Task SpeakAsync(string line, CancellationToken cancellationToken)
    {
        var cameraPosition = _graphics.GetCameraPosition();
        var textPosition = Sprite.Position.Offset(0, -Sprite.Size.Height - 20);

        var marginLeft = textPosition.X - cameraPosition.X;
        var marginRight = _graphics.Width - marginLeft;

        var wordWrapWidth = (int)Math.Min(marginLeft, marginRight) * 2;

        // TODO Must take into account real width of the text!

        // Move the textbox if we're to close to an edge of the screen.
        if (wordWrapWidth < MIN_WORD_WRAP_WIDTH)
        {
            wordWrapWidth = MIN_WORD_WRAP_WIDTH;

            if (marginLeft < marginRight)
            {
                textPosition = new Point(
                    wordWrapWidth / 2,
                    textPosition.Y);
            }
            else
            {
                textPosition = new Point(
                    _graphics.Width - (wordWrapWidth / 2) + cameraPosition.X, 
                    textPosition.Y);
            }
        }

        var animate = (Actor.FacingDirection == Direction.Front);
        if (animate)
        {
            Sprite.PlayAnimation(ANIM_TALK);
        }

        using var text = _graphics.AddText(
            line,
            textPosition,
            options =>
            {
                options.Depth = _graphics.Height;
                options.Origin = new Point(0.5, 0.5);
                options.FillColor = Actor.TextColor;
                options.WordWrapWidth = wordWrapWidth;
            });

        var delay = Math.Max(line.Length * DELAY_PER_CHAR, MIN_TEXT_DELAY);
        try
        {
            await Task.Delay(delay, cancellationToken);
        }
        catch (TaskCanceledException)
        {}

        if (animate)
        {
            Sprite.StopAnimation();
            FaceDirection(Direction.Front);
        }
    }

    public void FaceDirection(Direction direction)
    {
        Sprite.SetFrame(GetFrameName(Actor, direction));
    }

    public async Task WalkAsync(
        IEnumerable<Point> path,
        Direction faceDirection,
        CancellationToken cancellationToken)
    {
        foreach (var target in path)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var animation = (Sprite.Position.X < target.X)
                ? ANIM_WALK_RIGHT : ANIM_WALK_LEFT;

            Sprite.PlayAnimation(animation);

            var duration = Point.DistanceBetween(Sprite.Position, target) *
                WALK_SPEED_FACTOR;

            await Sprite.MoveAsync(
                target,
                duration,
                () => Sprite.SetDepth(Sprite.Position.Y),
                cancellationToken);
        }

        // If cancellation is requested, we're going to move to a
        // different location. Leaving the animation running makes the
        // transition smoother.
        if (!cancellationToken.IsCancellationRequested)
        {
            FaceDirection(faceDirection);
            Sprite.StopAnimation();
        }
    }

    private static string GetFrameName(Actor actor, Direction faceDirection) =>
        actor.IsVisible
            ? $"actors/{actor.Id}/{faceDirection.ToString().ToLowerInvariant()}"
            : "./transparent";

    public void Dispose()
    {
        Sprite.Dispose();
    }
}


///////////


//         // this.text = this.game.add.text(x, y - this.sprite.height - 40, "", this.createTextStyle(0));
//         // this.text.anchor.setTo(0.5);
//         // this.text.lineSpacing = -30;
//         // this.text.scale.x = 0.5;
//         // this.text.scale.y = 0.5; 



//         this.updateScale();

//         this.spaceKey = game.input.keyboard.addKey(Phaser.Keyboard.SPACEBAR);

//         this.spaceKey.onDown.add(
//             () => {
//                 if (this.resolveWaitForLine) {
//                     this.resolveWaitForLine();
//                     this.resolveWaitForLine = null;
//                 }
//             });
//     }

//     public async sayLine(text: string) {

//         // TODO Constant
//         if (this.faceDirection == "front") {
//             this.sprite.animations.play("talk");
//         }

//         const spaceLeft = this.sprite.x - this.game.camera.x;
//         const spaceRight = this.game.camera.x + this.game.camera.width - this.sprite.x;

//         const maxWordWrap = Math.min(spaceLeft, spaceRight) * 2;

//         this.textBox = this.game.add.text(this.sprite.x, 0, text, this.createTextStyle(maxWordWrap));
//         this.textBox.anchor.setTo(0.5);
//         this.textBox.lineSpacing = -30;
//         this.textBox.scale.x = 0.5;
//         this.textBox.scale.y = 0.5; 

//         this.textBox.y = this.sprite.y - this.sprite.height - (this.textBox.height / 2);


//         this.layers.text.add(this.textBox);

//     //  this.text.setText(text);

//         await this.waitWhileSpeaking(text.length);

//         // return new Promise((resolve) => {
            
//         //     this.game.time.events.add(
//         //         Math.max(text.length * gameInfo.textSpeed, gameInfo.minTextDuration),
//         //         () => {
//                     this.layers.text.remove(this.textBox, true);
// //                    this.textBox.kill();
// //                    this.text.setText('');
//                     if (this.faceDirection == "front") {
//                         this.sprite.animations.stop("talk", true);

//                         // Reset the frame.
//                         this.changeDirection(this.faceDirection);
//                     }
//         //             resolve();
//         //         });
//         // });
//     }

//     private waitWhileSpeaking(textLength: number) {
//         return new Promise((resolve) => {
            
//             this.resolveWaitForLine = resolve;

//             this.game.time.events.add(
//                 Math.max(textLength * gameInfo.textSpeed, gameInfo.minTextDuration),
//                 () => {
//                     if (this.resolveWaitForLine) {
//                         this.resolveWaitForLine();
//                         this.resolveWaitForLine = null;
//                     }
//                 });

//         });
//     }

//     public walkTo(path: Phaser.Point[], faceDirection: string): Promise<void> {

//         if (this.moveTween) {
//             this.moveTween.stop();
//         }

//         var i = 0;

//         return new Promise((resolve) => {

//             var move = () => {

//                 this.sprite.animations.stop();

//                 const animation = (this.sprite.x < path[i].x) ? "walk-right" : "walk-left";
//                 this.sprite.animations.play(animation);

//                 const tweenDuration = Phaser.Math.distance(this.sprite.x, this.sprite.y, path[i].x, path[i].y)
//                     * this.WALK_SPEED_FACTOR;

//                 this.moveTween = this.game.add.tween(this.sprite).to({ x: path[i].x, y: path[i].y }, tweenDuration).start();
        
//                 this.moveTween.onUpdateCallback(() => {
//                     this.updateScale();
//                     this.sprite.data.z = this.sprite.y;
//                 });
//                 this.moveTween.onComplete.add(() => {
//                     i += 1;
//                     if (i < path.length) {
//                         move();
//                     } else {
//                         this.moveTween = null;
//                         this.sprite.animations.stop();
//                         this.changeDirection(faceDirection);
//                         resolve();
//                     }
//                 });
//             };

//             move();
//         });
//     }

//     public update() {
// //        this.text.x = Math.floor(this.sprite.x);
//   //      this.text.y = Math.floor(this.sprite.y - this.sprite.height - 40);
//     }

//     public kill() {
//         this.sprite.destroy();
//     //    this.text.destroy();
//     }

//     private updateScale() {
//         if (this.scale) {
//             if (this.sprite.y < this.scale.start) {
//                 this.sprite.scale.set(this.scale.min / 100);
//             }
//             else if (this.sprite.y > this.scale.start && this.sprite.y < this.scale.end) {
//                 const scaleAreaHeight = this.scale.end - this.scale.start;
//                 const scaleDiff = this.scale.max - this.scale.min;
//                 const percentageInArea = (this.sprite.y - this.scale.start) / scaleAreaHeight;
//                 const scale = scaleDiff * percentageInArea + this.scale.min;
//                 this.sprite.scale.set(scale / 100);
//             }
//             else {
//                 this.sprite.scale.set(this.scale.max / 100);
//             }
//         }
//         else {
//             this.sprite.scale.set(1);
//         }
//     }

//     // TODO Extract
//     private createTextStyle(maxWordWrap: number) {

//         return {
//             font: "54px Onesize", // Using a large font-size and scaling it back looks better.
//             fill: this.talkColor,
//             stroke: "black",
//             strokeThickness: 12,
//             align: "center",
//             wordWrap: "true",
//             wordWrapWidth: Math.min(600, maxWordWrap) * 2 // Account for scaling.
//         };
//     }
// }
