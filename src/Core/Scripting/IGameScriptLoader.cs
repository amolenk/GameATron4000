namespace Amolenk.GameATron4000.Scripting;

public interface IGameScriptLoader
{
    IEnumerable<ScriptError> ScriptErrors { get; }

    Task<GameScript?> LoadFromManifestAsync(GameManifest manifest);

    void ClearErrors();
}
