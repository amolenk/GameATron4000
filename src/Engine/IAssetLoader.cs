namespace Amolenk.GameATron4000.Engine;

public interface IAssetLoader
{
    ValueTask LoadAtlasAsync(string key, string textureUrl, string atlasUrl);

    ValueTask LoadImageAsync(string key, string imageUrl);
}