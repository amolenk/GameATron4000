namespace Amolenk.GameATron4000.Engine.Scripting;

public record ScriptError(
    string Message,
    string Path,
    int Line,
    int Character);
