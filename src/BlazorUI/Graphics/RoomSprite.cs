namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class RoomSprite : IDisposable
{
    public Room Room { get; }
    public ISprite Sprite { get; }

    public RoomSprite(
        Room room,
        Func<Point, Task> onPointerDown,
        SpritesSpec spritesSpec,
        IGraphics graphics)
    {
        Room = room;

        var spriteInfo = spritesSpec.GetSpriteInfo(room.Id);

        // Add the room background.
        Sprite = graphics.AddSprite(
            spriteInfo.AtlasKey,
            spriteInfo.FrameName,
            new Point(0, 0),
            options =>
            {
                options.OnPointerDown = onPointerDown;
            });
    }

    public void Dispose()
    {
        Sprite.Dispose();
    }
}