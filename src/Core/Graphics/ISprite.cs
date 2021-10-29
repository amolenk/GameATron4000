namespace Amolenk.GameATron4000.Graphics;

public interface ISprite
{
    string Key { get; }

    Size Size { get; }

    Point Position { get; }

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

    void SetFrame(string frameName);

    ValueTask SetAnchorAsync(double value);

    void OnPointerDown(Action<Point> handler);

    ISpriteTween Move(
        Point target,
        int duration,
        Action<Point> onComplete);
}
