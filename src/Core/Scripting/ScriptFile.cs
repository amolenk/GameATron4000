namespace Amolenk.GameATron4000.Scripting;

public record ScriptFile(string Content, string Path)
{
    public int LineCount => Content.Split('\n').Length;
}
