namespace Amolenk.GameATron4000.Model.Manifest;

public class SpritesSpec : Dictionary<string, SpriteSpec>
{
    private const string DEFAULT_INVISIBLE_FRAME = "transparent";

    public (string AtlasKey, string FrameName, bool IsAnimation) GetSpriteInfo(
        string key,
        string frameName = WellKnownStatus.Default)
    {
        var atlasKey = "default";

        if (TryGetValue(key, out SpriteSpec spriteSpec))
        {
            atlasKey = spriteSpec.AtlasKey;

            if (spriteSpec.Animations.TryGetValue(
                frameName,
                out SpriteAnimationSpec animationSpec))
            {
                return (atlasKey, frameName, true);
            }

            if (spriteSpec.Frames.TryGetValue(
                frameName,
                out string actualFrameName))
            {
                return (atlasKey, actualFrameName, false);
            }
        }

        if (frameName == WellKnownStatus.Default)
        {
            return (atlasKey, key, false);
        }

        return (atlasKey, $"{key}/{frameName}", false);
    }
}
