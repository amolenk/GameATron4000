namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class GameObjectSprite : IDisposable
{
    private readonly SpritesSpec _spritesSpec;

    public IGameObject GameObject { get; }
    public ISprite Sprite { get; }
    protected IGraphics Graphics { get; private set; }

    public GameObjectSprite(
        IGameObject gameObject,
        SpritesSpec spritesSpec,
        IGraphics graphics,
        Func <IGameObject, Point, Task>? onPointerDown,
        Func <IGameObject, Point, Task>? onPointerOut,
        Func <IGameObject, Point, Task>? onPointerOver)
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
            GameObject.Status);

        Sprite.SetFrame(spriteInfo.FrameName);
    }

    private ISprite CreateSprite(
        Func <IGameObject, Point, Task>? onPointerDown,
        Func <IGameObject, Point, Task>? onPointerOut,
        Func <IGameObject, Point, Task>? onPointerOver)
    {
        var spriteInfo = _spritesSpec.GetSpriteInfo(
            GameObject.Id,
            GameObject.Status);

        var sprite = Graphics.AddSprite(
            spriteInfo.AtlasKey,
            spriteInfo.FrameName,
            GameObject.Position,
            options =>
            {
                options.Depth = GameObject.Position.Y + GameObject.DepthOffset;
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
// public class GameObjectSprite : GameObjectSprite <IGameObject>
// {
//     public GameObjectSprite(
//         GameObject gameObject,
//         SpritesSpec spritesSpec,
//         IGraphics graphics,
//         Func <IGameObject, Point, Task>? onPointerDown = null,
//         Func <IGameObject, Point, Task>? onPointerOut = null,
//         Func <IGameObject, Point, Task>? onPointerOver = null)
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
