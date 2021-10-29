namespace Amolenk.GameATron4000.Graphics;

public interface IGraphics
{
    int Width { get; }

    int Height { get; }

    ValueTask AddImageAsync(
        int x,
        int y,
        string key);

    ISprite AddSprite(
        string textureKey,
        string frameKey,
        Point position,
        Action<SpriteOptions>? configure = null);

    ValueTask AddTextAsync(int x, int y, string text);

    void DrawLines(IEnumerable<Line> lines, int lineWidth, int color);

    void SetWorldBounds(Size size);

    void StartCameraFollow(ISprite sprite);
}
