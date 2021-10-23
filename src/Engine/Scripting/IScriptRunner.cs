namespace Amolenk.GameATron4000.Engine.Scripting;

// TODO Dispose
public interface IScriptRunner<TGlobals> where TGlobals : class
{
    Task RunAsync(TGlobals globals);
}
