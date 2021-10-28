using System.Reflection;
using Amolenk.GameATron4000.BlazorClient.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureLogging(builder);

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

// TODO Still necessary?
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

builder.Services
    .AddSingleton<CustomServiceFactory>()
    .AddSingleton<ICustomMediator, CustomMediator>();

builder.Services.AddHttpClient<GameCatalog>(
    client => client.BaseAddress = baseAddress);

await builder.Build().RunAsync();

// TODO inline
static void ConfigureLogging(
  WebAssemblyHostBuilder builder,
  string section = "Logging")
{
    builder.Logging.AddConfiguration(builder.Configuration.GetSection(section));
}