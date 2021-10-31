namespace Amolenk.GameATron4000.Graphics;

public interface ISprite : IDisposable
{
    string Key { get; }
    Point Position { get; }
    Size Size { get; }

    Task MoveAsync(
        Point target,
        double duration,
        Action onUpdate,
        CancellationToken cancellationToken);

    void SetDepth(double depth);

    void SetFrame(string frameName);

    void AddAnimation(
        string key,
        string framePrefix,
        int frameStart,
        int frameEnd,
        int frameZeroPad,
        int frameRate,
        int repeat = -1,
        int repeatDelay = 0);

    void PlayAnimation(string key);

    void StopAnimation();
}
