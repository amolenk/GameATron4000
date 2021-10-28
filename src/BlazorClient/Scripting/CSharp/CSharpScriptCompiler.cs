namespace Amolenk.GameATron4000.Engine.Scripting.CSharp;

public class CSharpScriptCompiler : IScriptCompiler
{
    private readonly MetadataReference[] _references;

    private CSharpScriptCompiler(MetadataReference[] references)
    {
        _references = references;
    }

    public static async Task<CSharpScriptCompiler> CreateAsync(
        HttpClient client)
    {
        var references = await LoadMetadataReferencesAsync(client);

        return new CSharpScriptCompiler(references);
    }

    public IScriptRunner<TGlobals> Compile<TGlobals>(
        IEnumerable<ScriptFile> sources)
        where TGlobals : class
    {
        var mergedContent = string.Join(
            Environment.NewLine,
            sources.Select(source => source.Content));

        var syntaxTree = CSharpSyntaxTree.ParseText(
            mergedContent,
            CSharpParseOptions.Default
                .WithKind(SourceCodeKind.Script)
                .WithLanguageVersion(LanguageVersion.Preview));

        // Disable concurrentBuild to avoid PlatformNotSupportedException:
        // "Cannot wait on monitors on this runtime." error
        var compilationOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            concurrentBuild: false,
            usings: new[]
            {
                "Amolenk.GameATron4000.Engine.Graphics",
                "Amolenk.GameATron4000.Engine.Graphics.Geometry"
            });

        var compilation = CSharpCompilation.CreateScriptCompilation(
            Guid.NewGuid().ToString("N"),
            globalsType: typeof(TGlobals))
            .WithOptions(compilationOptions)
            .AddReferences(_references)
            .AddSyntaxTrees(syntaxTree);

        if (compilation.GetDiagnostics().Any(
            diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
        {
            throw new ScriptException(
                "Failed to compile script.",
                compilation.GetDiagnostics().Select(diagnostic =>
                    CSharpScriptError.FromDiagnostic(diagnostic, sources)));
        }

        return CSharpScriptRunner<TGlobals>.Create(compilation);
    }

    private static Task<MetadataReference[]> LoadMetadataReferencesAsync(
        HttpClient client) =>
        Task.WhenAll(AssemblyLoadContext.Default.Assemblies
            .Where(assembly => !assembly.IsDynamic)
            .Select(assembly => LoadMetadataReferenceAsync(
                assembly.GetName().Name!,
                client)));

    private static async Task<MetadataReference> LoadMetadataReferenceAsync(
        string assemblyName, HttpClient client)
    {
        var response = await client.GetAsync($"_framework/{assemblyName}.dll");

        using var stream = await response.Content.ReadAsStreamAsync();
        return MetadataReference.CreateFromStream(stream);
    }

    private CSharpCompilation Compile(
        IEnumerable<ScriptFile> sources,
        Type globalType)
    {
        var syntaxTrees = sources.Select(source =>
            CSharpSyntaxTree.ParseText(
            source.Content,
            CSharpParseOptions.Default
                .WithKind(SourceCodeKind.Script)
                .WithLanguageVersion(LanguageVersion.Preview),
            source.Path));

        var compilation = CSharpCompilation.CreateScriptCompilation(
            Guid.NewGuid().ToString("N"),
            globalsType: globalType)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(_references)
            .AddSyntaxTrees(syntaxTrees);

        if (compilation.GetDiagnostics().Any(
            diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
        {
            throw new ScriptException(
                "Failed to compile script.",
                compilation.GetDiagnostics().Select(diagnostic =>
                    CSharpScriptError.FromDiagnostic(diagnostic, sources)));
        }

        return compilation;
    }
}
