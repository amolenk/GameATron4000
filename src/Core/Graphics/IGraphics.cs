namespace Amolenk.GameATron4000.Graphics;

public interface IGraphics : IDisposable
{
    int Width { get; }

    int Height { get; }

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

    void CameraFollow(ISprite sprite);

    Task StartCameraFollowAsync(ISprite sprite);

    IDisposable Pause();
}
