namespace Amolenk.GameATron4000.Engine.Graphics;

public interface ISprite
{
    ValueTask SetAnchorAsync(double value);

    ValueTask OnPointerDownAsync(Func<Task> handler);
}
