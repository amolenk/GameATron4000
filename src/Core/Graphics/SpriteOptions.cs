namespace Amolenk.GameATron4000.Graphics;

public class SpriteOptions
{
    public Point Origin { get; set; } = new Point(0, 0);
    public double Depth { get; set; } = 0;
    public Func<Point, Task>? OnPointerOver { get; set; }
    public Func<Point, Task>? OnPointerOut { get; set; }
    public Func<Point, Task>? OnPointerDown { get; set; }
}
