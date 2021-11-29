namespace Amolenk.GameATron4000.Infrastructure.Repositories;

public class IndexedDBSaveGameRepository : ISaveGameRepository
{
    public const string StoreName = "SaveGame";

    private readonly IndexedDBManager _dbManager;

    public IndexedDBSaveGameRepository(IndexedDBManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task SaveGameAsync(string gameId, int slot, GameState state)
    {
        var saveGameRecord = new StoreRecord<SavedGame>
        {
            Storename = StoreName,
            Data = SavedGame.Create(gameId, slot, state)
        };

        if (await SavedGameExists(gameId, slot))
        {
            await _dbManager.UpdateRecord(saveGameRecord);
        }
        else
        {
            await _dbManager.AddRecord(saveGameRecord);
        }
    }

    public async Task<GameState?> LoadGameAsync(string gameId, int slot)
    {
        var savedGame = await _dbManager.GetRecordById<string, SavedGame>(
            _dbManager.Stores[0].Name, SavedGame.CreateId(gameId, slot));

        if (savedGame is not null)
        {
            return savedGame.State;
        }

        return null;
    }

    private async Task<bool> SavedGameExists(string gameId, int slot)
    {
        var state = await LoadGameAsync(gameId, slot);

        return state is not null;
    }
}