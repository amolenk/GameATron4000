namespace Amolenk.GameATron4000.Model.Meta;

public interface IGameManifestRepository
{
    Task<IReadOnlyCollection<GameManifest>> LoadGameManifestsAsync();

    Task<GameManifest?> TryLoadGameManifest(int diskNumber);
}
