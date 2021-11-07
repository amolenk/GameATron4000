namespace Amolenk.GameATron4000.Model;

public class SpriteSpec
{
    public string AtlasKey { get; set; } = string.Empty;

    public SpriteFramesSpec Frames { get; set; } = new();

    public SpriteAnimationsSpec Animations { get; set; } = new();
}
