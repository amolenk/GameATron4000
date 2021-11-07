namespace Amolenk.GameATron4000.Model.Manifest;

public class SpritesSpec : Dictionary<string, SpriteSpec>
{
    private const string DEFAULT_INVISIBLE_FRAME = "transparent";

    public (string AtlasKey, string FrameName) GetSpriteInfo(
        string key,
        string state = WellKnownState.Default)
    {
        var atlasKey = "default";

        if (TryGetValue(key, out SpriteSpec spriteSpec))
        {
            atlasKey = spriteSpec.AtlasKey;

            if (spriteSpec.Frames.TryGetValue(state, out string frameName))
            {
                return (atlasKey, frameName);
            }
        }

        if (state == WellKnownState.Default)
        {
            return (atlasKey, key);
        }

        if (state == WellKnownState.Invisible)
        {
            return (atlasKey, DEFAULT_INVISIBLE_FRAME);
        }

        return (atlasKey, $"{key}/{state}");
    }
}
