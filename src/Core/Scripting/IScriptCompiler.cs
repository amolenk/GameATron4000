namespace Amolenk.GameATron4000.Scripting;

public interface IScriptCompiler
{
    IScriptRunner<TGlobals> Compile<TGlobals>(IEnumerable<ScriptFile> sources)
        where TGlobals : class;
}
