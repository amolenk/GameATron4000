namespace Amolenk.GameATron4000.Model.Manifest;

public class SpritesSpec : Dictionary<string, SpriteSpec>
{
    private const string DEFAULT_INVISIBLE_FRAME = "transparent";

    public (string AtlasKey, string FrameName) GetSpriteInfo(
        string key,
        string frameName = WellKnownStatus.Default)
    {
        var atlasKey = "default";

        if (TryGetValue(key, out SpriteSpec spriteSpec))
        {
            atlasKey = spriteSpec.AtlasKey;

            if (spriteSpec.Frames.TryGetValue(
                frameName,
                out string actualFrameName))
            {
                return (atlasKey, actualFrameName);
            }
        }

        if (frameName == WellKnownStatus.Default)
        {
            return (atlasKey, key);
        }

        return (atlasKey, $"{key}/{frameName}");
    }
}
