namespace Amolenk.GameATron4000.Infrastructure.CSharpScripting;

public class CSharpScriptLoader : IGameScriptLoader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private Task<List<MetadataReference>> _loadMetadataReferencesTask;

    public IEnumerable<ScriptError> ScriptErrors { get; private set; }

    public CSharpScriptLoader(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        ScriptErrors = Enumerable.Empty<ScriptError>();
    }

    public Task PreloadAsync() => LoadMetadataReferencesAsync();

    public async Task<GameScript?> LoadFromManifestAsync(GameManifest manifest)
    {
        GameScript? result = null;

        try
        {
            Console.WriteLine("Loading game script!!!");

            result = await LoadGameScriptAsync(manifest);

            Console.WriteLine("Succes!!!");
        }
        catch (ScriptException ex)
        {
            Console.WriteLine("Fail!!!");
            ScriptErrors = ex.Errors;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fail!!!");
            ScriptErrors = new []
            {
                new ScriptError(ex.Message, string.Empty, 0, 0)
            };
        }

        return result;
    }

    public void ClearErrors()
    {
        ScriptErrors = Enumerable.Empty<ScriptError>();
    }

    private async Task<GameScript> LoadGameScriptAsync(GameManifest manifest)
    {
        List<ScriptFile> sources = new();

        var httpClient = _httpClientFactory.CreateClient("Default");

        foreach (var sourceUrl in manifest.Spec.Scripts)
        {
            // Add current time to prevent caching.
            var content = await httpClient.GetStringAsync(
                $"{manifest.BasePath}{sourceUrl}?time={DateTime.Now}");

            sources.Add(new ScriptFile(content, sourceUrl));
        }

        var metadataReferences = await LoadMetadataReferencesAsync();

        var compilation = Compile(sources, metadataReferences);

        EventQueue eventQueue = new();
        Game game = new(eventQueue);

        var assemblyLoadContext = await RunAsync(compilation, game);

        return new GameScript(game, eventQueue, assemblyLoadContext);
    }

    private Task<List<MetadataReference>> LoadMetadataReferencesAsync()
    {
        if (_loadMetadataReferencesTask is null)
        {
            _loadMetadataReferencesTask = Task.Run<List<MetadataReference>>(
                async () =>
                {
                    List<MetadataReference> references = new();

                    var httpClient = _httpClientFactory.CreateClient("Default");

                    foreach (var assembly in AssemblyLoadContext.Default.Assemblies)
                    {
                        if (!assembly.IsDynamic)
                        {
                            var response = await httpClient.GetAsync(
                                $"_framework/{assembly.GetName().Name!}.dll");

                            using var stream = await response.Content.ReadAsStreamAsync();
                            references.Add(MetadataReference.CreateFromStream(
                                stream));
                        }
                    }

                    return references;
                });
        }

        return _loadMetadataReferencesTask;
    }

    private CSharpCompilation Compile(
        IEnumerable<ScriptFile> sources,
        IEnumerable<MetadataReference> references)
    {
        var mergedContent = string.Join(
            Environment.NewLine,
            sources.Select(source => source.Content));

        var syntaxTree = CSharpSyntaxTree.ParseText(
            mergedContent,
            CSharpParseOptions.Default
                .WithKind(SourceCodeKind.Script)
                .WithLanguageVersion(LanguageVersion.Preview));

        var compilationOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            concurrentBuild: false,
            usings: new[]
            {
                "Amolenk.GameATron4000.Graphics",
                "Amolenk.GameATron4000.Graphics.Geometry",
                "Amolenk.GameATron4000.Model",
                "Amolenk.GameATron4000.Model.Builders",
                "System",
                "System.Linq"
            });

        var compilation = CSharpCompilation.CreateScriptCompilation(
            Guid.NewGuid().ToString("N"),
            globalsType: typeof(Game))
            .WithOptions(compilationOptions)
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

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

    
    private async Task<AssemblyLoadContext> RunAsync(
        Compilation compilation,
        Game game)
    {
        using var assemblyStream = EmitToStream(compilation);

        var assemblyLoadContext = new CustomAssemblyLoadContext();
        var assembly = assemblyLoadContext.LoadFromStream(assemblyStream);

        var entryPoint = compilation.GetEntryPoint(CancellationToken.None);
        if (entryPoint == null)
        {
            throw new ScriptException("Cannot find script entry point.");
        }

        var type = assembly.GetType(
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

        // Submission array must be large enough to hold the globals object +
        // the state of each submission. As we only run the script once, 2 is
        // enough here.
        // See: https://github.com/dotnet/roslyn/blob/main/src/Scripting/Core/ScriptExecutionState.cs
        var submissionArray = new object[2];
        submissionArray[0] = game;

        await submission(submissionArray);

        return assemblyLoadContext;
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
