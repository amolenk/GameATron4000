namespace Amolenk.GameATron4000.Model.Manifest;

public class SpriteSpec
{
    public string AtlasKey { get; set; } = "default";

    public SpriteFramesSpec Frames { get; set; } = new();

    public SpriteAnimationsSpec Animations { get; set; } = new();
}
