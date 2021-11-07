namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public abstract class GameObjectSprite<TObject> : IDisposable
    where TObject : GameObject
{
    private readonly SpriteSpec _spriteSpec;

    public TObject GameObject { get; }
    public ISprite Sprite { get; }
    protected IGraphics Graphics { get; private set; }

    protected GameObjectSprite(
        TObject gameObject,
        SpriteSpec spriteSpec,
        IGraphics graphics,
        Func<GameObject, Point, Task>? onPointerDown,
        Func<GameObject, Point, Task>? onPointerOut,
        Func<GameObject, Point, Task>? onPointerOver)
    {
        _spriteSpec = spriteSpec;

        GameObject = gameObject;
        Graphics = graphics;

        Sprite = CreateSprite(
            onPointerDown,
            onPointerOut,
            onPointerOver);
    }

    public void Dispose()
    {
        Sprite.Dispose();
    }

    protected void ResetSpriteFrame()
    {
        Sprite.SetFrame(_spriteSpec.Frames[GameObject.State]);
    }

    private ISprite CreateSprite(
        Func<GameObject, Point, Task>? onPointerDown,
        Func<GameObject, Point, Task>? onPointerOut,
        Func<GameObject, Point, Task>? onPointerOver)
    {
        var sprite = Graphics.AddSprite(
            _spriteSpec.AtlasKey,
            _spriteSpec.Frames[GameObject.State],
            GameObject.Position,
            options =>
            {
                options.Depth = GameObject.Position.Y;
                options.Origin = new Point(0.5, 1);

                if (GameObject.IsTouchable)
                {
                    if (onPointerDown is not null)
                    {
                        options.OnPointerDown = (pointerPosition) =>
                            onPointerDown(GameObject, pointerPosition);
                    }

                    if (onPointerOut is not null)
                    {
                        options.OnPointerOut = (pointerPosition) =>
                            onPointerOut(GameObject, pointerPosition);
                    }

                    if (onPointerOver is not null)
                    {
                        options.OnPointerOver = (pointerPosition) =>
                            onPointerOver(GameObject, pointerPosition);
                    }
                }
            });

        // Load animations.
        foreach (var animationSpec in _spriteSpec.Animations)
        {
            sprite.AddAnimation(
                animationSpec.Key,
                _spriteSpec.AtlasKey,
                animationSpec.Value);
        }

        return sprite;
    }
}

public class GameObjectSprite : GameObjectSprite<GameObject>
{
    public GameObjectSprite(
        GameObject gameObject,
        SpriteSpec spriteSpec,
        IGraphics graphics,
        Func<GameObject, Point, Task>? onPointerDown = null,
        Func<GameObject, Point, Task>? onPointerOut = null,
        Func<GameObject, Point, Task>? onPointerOver = null)
        : base(
            gameObject,
            spriteSpec,
            graphics,
            onPointerDown,
            onPointerOut,
            onPointerOver)
    {
    }
}
