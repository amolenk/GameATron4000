namespace Amolenk.GameATron4000.Infrastructure.Repositories;

public class LocalStorageSaveGameRepository : ISaveGameRepository
{
    public const string StoreName = "SaveGame";

    private readonly ILocalStorageService _localStorageService;
    private readonly IJSRuntime _jsRuntime;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public LocalStorageSaveGameRepository(ILocalStorageService localStorageService, IJSRuntime jsRuntime)
    {
        _localStorageService = localStorageService;
        _jsRuntime = jsRuntime;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task SaveGameAsync(string gameId, int slot, GameState state)
    {
        var slotName = $"{gameId}#{slot}";

        await _localStorageService.SetItemAsync(slotName, state);
    }

    public async Task<GameState?> LoadGameAsync(string gameId, int slot)
    {
        var slotName = $"{gameId}#{slot}";

        if (await _localStorageService.ContainKeyAsync(slotName))
        {
            return await _localStorageService.GetItemAsync<GameState>(slotName);
        }

        return null;
    }
}