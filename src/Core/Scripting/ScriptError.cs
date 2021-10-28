namespace Amolenk.GameATron4000.Scripting;

public record ScriptError(
    string Message,
    string Path,
    int Line,
    int Character);
