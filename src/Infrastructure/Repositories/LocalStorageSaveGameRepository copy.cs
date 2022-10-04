namespace Amolenk.GameATron4000.Infrastructure.Repositories;

public class LocalStorageSaveGameRepository : ISaveGameRepository
{
    public const string StoreName = "SaveGame";

    private readonly IJSRuntime _jsRuntime;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public LocalStorageSaveGameRepository(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task SaveGameAsync(string gameId, int slot, GameState state)
    {
        var slotName = $"{gameId}#{slot}";
        var savedGame = JsonSerializer.Serialize(state, _jsonSerializerOptions);

        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", slotName, savedGame);
    }

    public async Task<GameState?> LoadGameAsync(string gameId, int slot)
    {
        var slotName = $"{gameId}#{slot}";
        var savedGame = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", slotName);

        if (savedGame is not null)
        {
            return JsonSerializer.Deserialize<GameState>(savedGame);
        }

        return null;
    }
}