namespace Amolenk.GameATron4000.Graphics;

public interface ISprite : IDisposable
{
    string Key { get; }
    Point Position { get; }
    Size Size { get; }
    string Frame { get; }

    void SetDepth(double depth);

    void SetFrame(string frameName);

    Task MoveAsync(
        Point target,
        double duration,
        Action onUpdate,
        CancellationToken cancellationToken);

    void AddAnimation(
        string key,
        string atlasKey,
        SpriteAnimationSpec animation);

    void PlayAnimation(string key);

    void StopAnimation();
}
