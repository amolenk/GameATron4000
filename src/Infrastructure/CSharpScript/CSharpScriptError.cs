namespace Amolenk.GameATron4000.Infrastructure.CSharpScripting;

public class CSharpScriptError
{
    public static ScriptError FromDiagnostic(
        Diagnostic diagnostic,
        IEnumerable<ScriptFile> sources)
    {
        var location = diagnostic.Location;
        if (location != Location.None)
        {
            var lineSpan = location.GetLineSpan();
            if (lineSpan.IsValid)
            {
                var position = lineSpan.StartLinePosition;
                var path = string.Empty;
                var line = position.Line + 1; // position.Line is zero-based

                foreach (var file in sources)
                {
                    path = file.Path;

                    if (line > file.LineCount)
                    {
                        line -= file.LineCount;
                    }
                    else
                    {
                        break;
                    }
                }

                return new ScriptError(
                    diagnostic.GetMessage(),
                    path,
                    line,
                    position.Character);
            }
        }

        return new ScriptError(
            diagnostic.GetMessage(),
            string.Empty,
            0,
            0);
    }
}
