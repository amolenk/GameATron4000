namespace Amolenk.GameATron4000.Graphics;

public class SpriteOptions
{
    public bool IsInteractive { get; private set; } = false;

    public double OriginX { get; private set; } = 0;

    public double OriginY { get; private set; } = 0;

    public void SetInteractive()
    {
        IsInteractive = true;
    }

    public void SetOrigin(double x = 0, double y = 0)
    {
        OriginX = x;
        OriginY = y;
    }
}
