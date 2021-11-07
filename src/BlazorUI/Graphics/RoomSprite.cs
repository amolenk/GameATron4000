namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class RoomSprite : IDisposable
{
    public Room Room { get; }
    public ISprite Sprite { get; }

    public RoomSprite(
        Room room,
        Func<Point, Task> onPointerDown,
        SpriteSpec spriteSpec,
        IGraphics graphics)
    {
        Room = room;

        // Add the room background.
        Sprite = graphics.AddSprite(
            spriteSpec.AtlasKey,
            spriteSpec.Frames[WellKnownState.Default],
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