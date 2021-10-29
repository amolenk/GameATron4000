namespace Amolenk.GameATron4000.Infrastructure.Phaser;

public interface ICamera
{
    void Follow(ISprite sprite, bool jumpTo = false);
}