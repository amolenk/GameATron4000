namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class RoomGraphics
{
    public Room Room { get; }
    public ISprite Sprite { get; }

    private RoomGraphics(Room room, ISprite sprite)
    {
        Room = room;
        Sprite = sprite;
    }

    public static RoomGraphics Create(
        Room room,
        Func<Point, Task> onPointerDown,
        IGraphics graphics)
    {
        // Add the room background.
        var sprite = graphics.AddSprite(
            "images", // TODO
            $"rooms/{room.Id}",
            new Point(0, 0),
            options =>
            {
                options.OnPointerDown = onPointerDown;
            });

        return new RoomGraphics(room, sprite);
    }
}