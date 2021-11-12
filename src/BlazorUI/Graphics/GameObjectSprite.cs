namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class GameObjectSprite : IDisposable
{
    private readonly SpritesSpec _spritesSpec;

    public GameObject GameObject { get; }
    public ISprite Sprite { get; }
    protected IGraphics Graphics { get; private set; }

    public GameObjectSprite(
        GameObject gameObject,
        SpritesSpec spritesSpec,
        IGraphics graphics,
        Func<GameObject, Point, Task>? onPointerDown,
        Func<GameObject, Point, Task>? onPointerOut,
        Func<GameObject, Point, Task>? onPointerOver)
    {
        _spritesSpec = spritesSpec;

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

    // public void SetSpriteFrame(string frameName)
    // {
    //     var spriteInfo = _spritesSpec.GetSpriteInfo(
    //         GameObject.Id,
    //         GameObject.State);

    //     Sprite.SetFrame(spriteInfo.FrameName);
    // }

    public void ResetSpriteFrame()
    {
        var spriteInfo = _spritesSpec.GetSpriteInfo(
            GameObject.Id,
            GameObject.Frame);

        Sprite.SetFrame(spriteInfo.FrameName);
    }

    private ISprite CreateSprite(
        Func<GameObject, Point, Task>? onPointerDown,
        Func<GameObject, Point, Task>? onPointerOut,
        Func<GameObject, Point, Task>? onPointerOver)
    {
        var spriteInfo = _spritesSpec.GetSpriteInfo(
            GameObject.Id,
            GameObject.Frame);

        var sprite = Graphics.AddSprite(
            spriteInfo.AtlasKey,
            spriteInfo.FrameName,
            GameObject.Position,
            options =>
            {
                options.Depth = GameObject.Position.Y;
                options.Origin = new Point(0.5, 1);
                options.ScrollFactor = GameObject.ScrollFactor;

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
        if (_spritesSpec.TryGetValue(GameObject.Id, out SpriteSpec spriteSpec))
        {
            foreach (var animationSpec in spriteSpec.Animations)
            {
                sprite.AddAnimation(
                    animationSpec.Key,
                    spriteSpec.AtlasKey,
                    animationSpec.Value);
            }
        }

        return sprite;
    }
}

// TODO
// public class GameObjectSprite : GameObjectSprite<GameObject>
// {
//     public GameObjectSprite(
//         GameObject gameObject,
//         SpritesSpec spritesSpec,
//         IGraphics graphics,
//         Func<GameObject, Point, Task>? onPointerDown = null,
//         Func<GameObject, Point, Task>? onPointerOut = null,
//         Func<GameObject, Point, Task>? onPointerOver = null)
//         : base(
//             gameObject,
//             spritesSpec,
//             graphics,
//             onPointerDown,
//             onPointerOut,
//             onPointerOver)
//     {
//     }
// }
