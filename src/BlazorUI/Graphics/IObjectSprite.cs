namespace Amolenk.GameATron4000.BlazorUI.Graphics;

public interface IObjectSprite : IDisposable
{
    public string Id { get; }

    public ISprite Sprite { get; }

    public void UpdateSpriteFrameForStatus(string status);
}
