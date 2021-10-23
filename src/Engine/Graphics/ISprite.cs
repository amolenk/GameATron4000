namespace Amolenk.GameATron4000.Engine.Graphics;

public interface ISprite
{
    int Width { get; }

    int Height { get; }

    ValueTask SetAnchorAsync(double value);

    ValueTask OnPointerDownAsync(Func<Task> handler);
}
