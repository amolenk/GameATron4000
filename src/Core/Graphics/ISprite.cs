namespace Amolenk.GameATron4000.Graphics;

public interface ISprite
{
    string Id { get; }

    Point Position { get; }
    int Width { get; }

    int Height { get; }

    ValueTask SetAnchorAsync(double value);

    ValueTask OnPointerDownAsync(Func<Point, Task> handler);

    ISpriteTween Move(
        Point target,
        int duration,
        Action<Point> onUpdate,
        Action<Point> onComplete);
}
