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

    IText AddText(
        string text,
        Point position,
        Action<TextOptions>? configure = null);

    void DrawLines(IEnumerable<Line> lines, int lineWidth, int color);

    Point GetCameraPosition();

    void SetCameraBounds(Size size);

    void StartCameraFollow(ISprite sprite);

    IDisposable Pause();
}
