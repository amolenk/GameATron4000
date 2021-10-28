namespace Amolenk.GameATron4000.Engine.Scripting.CSharp;

// TODO Dispose
public class CSharpScriptRunner<TGlobals> : IScriptRunner<TGlobals>
    where TGlobals : class
{
    private readonly Compilation _compilation;
    private readonly AssemblyLoadContext _assemblyLoadContext;
    private readonly Assembly _assembly;

    private CSharpScriptRunner(
        Compilation compilation,
        AssemblyLoadContext assemblyLoadContext,
        Assembly assembly)
    {
        _compilation = compilation;
        _assemblyLoadContext = assemblyLoadContext;
        _assembly = assembly;
    }

    public static CSharpScriptRunner<TGlobals> Create(
        Compilation compilation)
    {
        using var assemblyStream = EmitToStream(compilation);

        var assemblyLoadContext = new CustomAssemblyLoadContext();
        var assembly = assemblyLoadContext.LoadFromStream(assemblyStream);

        return new CSharpScriptRunner<TGlobals>(
            compilation,
            assemblyLoadContext,
            assembly);
    }

    public Task RunAsync(TGlobals globals)
    {
        var entryPoint = _compilation.GetEntryPoint(CancellationToken.None);
        if (entryPoint == null)
        {
            throw new ScriptException("Cannot find script entry point.");
        }

        var type = _assembly.GetType(
            $"{entryPoint.ContainingNamespace.MetadataName}.{entryPoint.ContainingType.MetadataName}");
        if (type == null)
        {
            throw new ScriptException(
                "Cannot find script entry point type.");
        }

        var entryPointMethod = type.GetMethod(entryPoint.MetadataName);
        if (entryPointMethod == null)
        {
            throw new ScriptException(
                "Cannot find script entry point method.");
        }

        var submission = (Func<object[], Task>)entryPointMethod.CreateDelegate(
            typeof(Func<object[], Task>));

        var submissionArray = new object[2 + 1];
        submissionArray[0] = globals;

        return submission(submissionArray);
    }

    private static Stream EmitToStream(Compilation compilation)
    {
        var outputStream = new MemoryStream();

        var result = compilation.Emit(outputStream);
        if (result.Success)
        {
            outputStream.Position = 0;

            return outputStream;
        }

        outputStream.Dispose();

        throw new ScriptException(
            "Failed to emit compilation to assembly stream.");
    }
}
