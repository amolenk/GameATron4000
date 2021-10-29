namespace Amolenk.GameATron4000.Model;

public class GameSpec
{
    public ImagesSpec Images { get; set; } = new();

    public string[] Scripts { get; set; } = Array.Empty<string>();
}
