namespace Amolenk.GameATron4000.Engine;

public class GameManifest
{
    public GameMetadata Metadata { get; set; } = new();

    public GameSpec Spec { get; set; } = new();

    public string BasePath { get; set; } = string.Empty;
}
