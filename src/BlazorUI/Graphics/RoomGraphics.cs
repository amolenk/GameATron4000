namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public class RoomGraphics
{
    private readonly Action<Point> _onPointerDown;
 
    public Room Room { get; }
    public ISprite Sprite { get; }

    private RoomGraphics(Room room, ISprite sprite, Action<Point> onPointerDown)
    {
        _onPointerDown = onPointerDown;

        Room = room;
        Sprite = sprite;

        sprite.OnPointerDown(OnPointerDown);
    }

    public static RoomGraphics Create(
        Room room,
        Action<Point> onPointerDown,
        IGraphics graphics)
    {
        // Add the room background.
        var sprite = graphics.AddSprite(
            "images", // TODO
            $"rooms/{room.Id}",
            new Point(0, 0),
            options =>
            {
                options.IsInteractive = true;
            });

        return new RoomGraphics(room, sprite, onPointerDown);
    }

    [JSInvokable]
    public void OnPointerDown(Point mousePosition) =>
        _onPointerDown(mousePosition);
}