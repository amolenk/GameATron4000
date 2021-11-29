namespace Amolenk.GameATron4000.Model.Manifest;

public class GameManifest
{
    public GameMetadata Metadata { get; set; } = new();

    public GameSpec Spec { get; set; } = new();

    public int DiskNumber { get; set; }

    public string BasePath { get; set; } = string.Empty;
}
