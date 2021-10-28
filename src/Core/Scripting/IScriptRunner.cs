namespace Amolenk.GameATron4000.Scripting;

// TODO Dispose
public interface IScriptRunner<TGlobals> where TGlobals : class
{
    Task RunAsync(TGlobals globals);
}
