namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class ItemSprite : ObjectSprite<Item>
{
    public ItemSprite(
        Item item,
        SpritesSpec spritesSpec,
        IGraphics graphics,
        Func<Item, Point, Task>? onPointerDown,
        Func<Item, Point, Task>? onPointerOut,
        Func<Item, Point, Task>? onPointerOver)
        : base(
            item,
            spritesSpec,
            graphics,
            onPointerDown,
            onPointerOut,
            onPointerOver)
    {
    }
}
