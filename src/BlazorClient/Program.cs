using System.Reflection;
using Amolenk.GameATron4000.BlazorClient.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureLogging(builder);

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

builder.Services.AddEngine(baseAddress);

await builder.Build().RunAsync();

static void ConfigureLogging(
  WebAssemblyHostBuilder builder,
  string section = "Logging")
{
    builder.Logging.AddConfiguration(builder.Configuration.GetSection(section));
}