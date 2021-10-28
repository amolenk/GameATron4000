namespace Amolenk.GameATron4000.Engine.Graphics;

public interface IGraphics
{
    int Width { get; }

    int Height { get; }

    ValueTask AddImageAsync(
        int x,
        int y,
        string key);

    ValueTask<ISprite> AddSpriteAsync(
        int x,
        int y,
        string key,
        Action<SpriteOptions>? configure = null);

    ValueTask AddTextAsync(int x, int y, string text);

    void DrawLines(IEnumerable<Line> lines, int lineWidth, int color);

    ValueTask SetWorldBoundsAsync(int x, int y, int width, int height);
}
