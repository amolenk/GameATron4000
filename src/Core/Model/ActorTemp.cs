namespace Amolenk.GameATron4000.Model;

public class ActorTemp
{
    public string Id;
    public string Name;
    public int PositionX;
    public int PositionY;

    public ActorTemp()
    {
    }
}

public class Actor
{
    private const int WALK_SPEED_FACTOR = 4;

    private readonly ISprite _sprite;

    public Point Position { get; private set; }

    private readonly IGraphics _graphics;

    private Stack<Point>? _walkPath;
    private ISpriteTween _walkTween;

    private Actor(int x, int y, ISprite sprite, IGraphics graphics)
    {
        _sprite = sprite;
        _graphics = graphics;

        Position = new Point(x, y);
    }

    public void Walk(IEnumerable<Line> path)
    {
        // https://rexrainbow.github.io/phaser3-rex-notes/docs/site/tween/

        // if (_walkTween != null)
        // {
        //     _walkTween.Stop();
        // }

        if (_walkTween != null)
        {
            _walkTween.Stop();
        }

        if (path.Any())
        {
            _walkPath = new Stack<Point>(path.Select(edge => edge.End).Reverse());

            TryWalkNextSegment();
        }
    }

    private void TryWalkNextSegment()
    {
        // TODO Dispose of used tweens

//        var walkFrom = _walkPath!.Pop();
        
        if (_walkPath.TryPop(out Point? walkTo))
        {
            var duration = (int)Point.DistanceBetween(Position, walkTo) * WALK_SPEED_FACTOR;

            _walkTween = _sprite.Move(
                walkTo,
                duration,
                (position) =>
                {
                    Position = position;
                },
                (position) => 
                {
                    Position = position;

                    TryWalkNextSegment();
                });
        }
        else
        {
            _walkPath = null;
        }
    }

    public static async Task<Actor> CreateAsync(
        ActorTemp data,
        IGraphics graphics)
    {
        var sprite = await graphics.AddSpriteAsync(
            data.PositionX,
            data.PositionY,
            $"actors/{data.Id}/front",
            options =>
            {
                options.SetOrigin(0.5, 1);
            });

        return new Actor(data.PositionX, data.PositionY, sprite, graphics);
    }
}