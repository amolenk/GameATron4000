namespace Amolenk.GameATron4000.Infrastructure.Repositories;

public interface ISaveGameRepository
{
    Task SaveGameAsync(string gameId, int slot, GameState state);

    Task<GameState?> LoadGameAsync(string gameId, int slot);
}