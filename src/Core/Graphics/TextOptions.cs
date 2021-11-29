namespace Amolenk.GameATron4000.Graphics;

public class TextOptions
{
    public double Depth { get; set; } = 0;
    public string FillColor { get; set; } = "white";
    public Point Origin { get; set; } = new Point(0, 0);
    public int ScrollFactor { get; set; } = -1;
    public int WordWrapWidth { get; set; } = 0;
}
