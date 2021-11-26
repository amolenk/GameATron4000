using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureLogging(builder);

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

// TODO Still necessary?
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

builder.Services
    .AddSingleton<IJSInProcessRuntime>(
        sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>())
    .AddSingleton<PhaserGraphicsFactory>()
    .AddTransient<IMediator, DefaultMediator>()
    .AddSingleton<IGameScriptLoader, CSharpScriptLoader>();

builder.Services.AddHttpClient(
    "Default",
    client => client.BaseAddress = baseAddress);

builder.Services.AddHttpClient<IGameManifestRepository, LocalGameManifestRepository>(
    client => client.BaseAddress = baseAddress);

// builder.Services.AddHttpClient<IGameScriptLoader, CSharpScriptLoader>(
//     client => client.BaseAddress = baseAddress);

await builder.Build().RunAsync();

// TODO inline
static void ConfigureLogging(
  WebAssemblyHostBuilder builder,
  string section = "Logging")
{
    builder.Logging.AddConfiguration(builder.Configuration.GetSection(section));
}
