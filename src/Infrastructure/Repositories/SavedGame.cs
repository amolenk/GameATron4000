namespace Amolenk.GameATron4000.Infrastructure.Repositories;

public class SavedGame
{
    public string Id { get; set; } = string.Empty;

    public GameState State { get; set; } = null!;

    internal static SavedGame Create(string gameId, int slot, GameState state) =>
        new SavedGame
        {
            Id = CreateId(gameId, slot),
            State = state
        };

    internal static string CreateId(string gameId, int slot) =>
        $"{gameId}:{slot:000}";
}