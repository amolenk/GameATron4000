namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class ActorSprite : ObjectSprite<Actor>
{
    private const int WALK_SPEED_FACTOR = 4;
    private const int MIN_WORD_WRAP_WIDTH = 400;

    private const int DELAY_PER_CHAR = 75;
    private const int MIN_TEXT_DELAY = 1500;

    public ActorSprite(
        Actor actor,
        Point position,
        string status,
        SpritesSpec spritesSpec,
        IGraphics graphics,
        Func<Actor, Point, Task>? onPointerDown = null,
        Func<Actor, Point, Task>? onPointerOut = null,
        Func<Actor, Point, Task>? onPointerOver = null)
        : base(
            actor,
            position,
            status,
            spritesSpec,
            graphics,
            onPointerDown,
            onPointerOut,
            onPointerOver)
    {
    }

    public async Task SayLineAsync(string line, string actorStatus, CancellationToken cancellationToken)
    {
        var cameraPosition = Graphics.GetCameraPosition();
        var textPosition = Sprite.Position.Offset(
            0,
            -Sprite.Size.Height - 20);
            //TODO GameObject.IsVisible ? -Sprite.Size.Height - 20 : 0);

        var marginLeft = textPosition.X - cameraPosition.X;
        var marginRight = Graphics.Width - marginLeft;

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
                    Graphics.Width - (wordWrapWidth / 2) + cameraPosition.X, 
                    textPosition.Y);
            }
        }

        var frameBeforeAnimation = Sprite.Frame;

        var animate = (actorStatus == WellKnownStatus.FaceCamera);
        if (animate)
        {
            Sprite.PlayAnimation(WellKnownAnimation.Talk);
        }

        using var text = Graphics.AddText(
            line,
            textPosition,
            options =>
            {
                options.Depth = Graphics.Height + 1; // Text must be top-most.
                options.FillColor = Model.TextColor;
                options.Origin = new Point(0.5, 0.5);
                options.ScrollFactor = Model.ScrollFactor;
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
            Sprite.SetFrame(frameBeforeAnimation);
        }
    }

    public async Task WalkAsync(
        IEnumerable<Point> path,
        string endInStatus,
        CancellationToken cancellationToken)
    {
        foreach (var target in path)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var animation = (Sprite.Position.X < target.X)
                ? WellKnownAnimation.WalkRight
                : WellKnownAnimation.WalkLeft;

            Sprite.PlayAnimation(animation);

            var duration = Point.DistanceBetween(Sprite.Position, target) *
                WALK_SPEED_FACTOR;

            await Sprite.MoveAsync(
                target,
                duration,
                () =>
                {
                    Sprite.SetDepth(Sprite.Position.Y);
                },
                cancellationToken);
        }

        // If cancellation is requested, we're going to move to a
        // different location. Leaving the animation running makes the
        // transition smoother.
        if (!cancellationToken.IsCancellationRequested)
        {
            Sprite.StopAnimation();
            UpdateSpriteFrameForStatus(endInStatus);
        }
    }
}


/////////// TODO

//         this.updateScale();

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
