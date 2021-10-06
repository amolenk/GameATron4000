namespace Amolenk.GameATron4000.Engine.Phaser;

public interface IGraphics
{
    ValueTask AddImageAsync(int x, int y, string texture, string? frame = null);

    ValueTask AddTextAsync(int x, int y, string text);

    ValueTask StartSceneAsync(string key);
}
