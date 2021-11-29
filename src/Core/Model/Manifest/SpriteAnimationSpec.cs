namespace Amolenk.GameATron4000.Model.Manifest;

public class SpriteAnimationSpec
{
    public string FramePrefix { get; set; } = string.Empty;
    public int FrameStart { get; set; }
    public int FrameEnd { get; set; }
    public int FrameZeroPadding { get; set; }
    public int FrameRate { get; set; }
    public int Repeat { get; set; } = -1;
    public int RepeatDelay { get; set; } = 0;
}
