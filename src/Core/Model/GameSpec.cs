namespace Amolenk.GameATron4000.Model;

public class GameSpec
{
    public AtlassesSpec Atlasses { get; set; } = new();

    public SpritesSpec Sprites { get; set; } = new();

    public string[] Scripts { get; set; } = Array.Empty<string>();
}
