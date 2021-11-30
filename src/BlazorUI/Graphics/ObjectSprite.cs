namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public abstract class ObjectSprite<TObject> : IObjectSprite
    where TObject : GameObject
{
    private readonly SpritesSpec _spritesSpec;

    public string Id => Model.Id;
    public TObject Model { get; }
    public ISprite Sprite { get; }
    protected IGraphics Graphics { get; private set; }

    protected ObjectSprite(
        TObject gameObject,
        Point position,
        string status,
        SpritesSpec spritesSpec,
        IGraphics graphics,
        Func<TObject, Point, Task>? onPointerDown,
        Func<TObject, Point, Task>? onPointerOut,
        Func<TObject, Point, Task>? onPointerOver)
    {
        _spritesSpec = spritesSpec;

        Model = gameObject;
        Graphics = graphics;

        Sprite = CreateSprite(
            position,
            status,
            onPointerDown,
            onPointerOut,
            onPointerOver);
    }

    public void Dispose()
    {
        Sprite.Dispose();
    }

    public void UpdateSpriteFrameForStatus(string status)
    {
        var spriteInfo = _spritesSpec.GetSpriteInfo(
            Model.Id,
            status);

        if (spriteInfo.IsAnimation)
        {
            Sprite.PlayAnimation(spriteInfo.FrameName);
        }
        else
        {
            Sprite.StopAnimation();
            Sprite.SetFrame(spriteInfo.FrameName);
        }
    }

    private ISprite CreateSprite(
        Point position,
        string status,
        Func<TObject, Point, Task>? onPointerDown,
        Func<TObject, Point, Task>? onPointerOut,
        Func<TObject, Point, Task>? onPointerOver)
    {
        var spriteInfo = _spritesSpec.GetSpriteInfo(
            Model.Id,
            status);

        var sprite = Graphics.AddSprite(
            spriteInfo.AtlasKey,
            spriteInfo.FrameName,
            position,
            options =>
            {
                options.Depth = position.Y + Model.DepthOffset;
                options.Origin = new Point(0.5, 1);
                options.ScrollFactor = Model.ScrollFactor;

                if (Model.IsTouchable)
                {
                    if (onPointerDown is not null)
                    {
                        options.OnPointerDown = (pointerPosition) =>
                            onPointerDown(Model, pointerPosition);
                    }

                    if (onPointerOut is not null)
                    {
                        options.OnPointerOut = (pointerPosition) =>
                            onPointerOut(Model, pointerPosition);
                    }

                    if (onPointerOver is not null)
                    {
                        options.OnPointerOver = (pointerPosition) =>
                            onPointerOver(Model, pointerPosition);
                    }
                }
            });

        // Load animations.
        if (_spritesSpec.TryGetValue(Model.Id, out SpriteSpec spriteSpec))
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
