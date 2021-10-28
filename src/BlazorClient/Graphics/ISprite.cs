namespace Amolenk.GameATron4000.Engine.Graphics;

public interface ISprite
{
    string Id { get; }

    Point Position { get; }
    int Width { get; }

    int Height { get; }

    ValueTask SetAnchorAsync(double value);

    ValueTask OnPointerDownAsync(Func<Point, Task> handler);

    ITween Move(
        Point target,
        int duration,
        Action<Point> onUpdate,
        Action<Point> onComplete);
}
