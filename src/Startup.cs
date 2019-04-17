using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameATron4000.BotFileAssistant;
using GameATron4000.Configuration;
using GameATron4000.Core;
using GameATron4000.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GameATron4000
{
    public class Startup
    {
        private readonly string _environmentName;

        public Startup(IHostingEnvironment env)
        {
            _environmentName = env.EnvironmentName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables(prefix: "GAMEATRON4000:");

            Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure custom options classes for Bot and LUIS configuration sections.
            services.Configure<GuiOptions>(Configuration.GetSection("GUI"));
            services.Configure<LUISOptions>(Configuration.GetSection("LUIS"));

            var botConfigPath = Configuration.GetValue<string>("Bot:FilePath");
            var botConfigKey = Configuration.GetValue<string>("Bot:FileSecret");
            // if (!File.Exists(botConfigPath))
            // {
            //     throw new FileNotFoundException($"The .bot configuration file was not found. botConfigPath: '{botConfigPath}'");
            // }

            // Loads .bot configuration file.
            var botConfig = File.Exists(botConfigPath)
                ? BotConfiguration.Load(botConfigPath, botConfigKey)
                : new BotConfiguration();

            // Add a singleton that the Bot can access through dependency injection.
            services.AddSingleton(sp => botConfig ?? throw new InvalidOperationException($"The .bot configuration file could not be loaded. botFilePath: {botConfigPath}"));

            // Initialize Bot Connected Services clients.
            var connectedServices = new BotServices(botConfig);
            services.AddSingleton(sp => connectedServices);

            services.AddBot<GameBot>(options =>
            {
                // Retrieve current endpoint.
                var service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint");
                if (service is EndpointService endpointService)
                {
                    options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
                }

                options.Middleware.Add(new BotFileAssistantMiddleware());

                // TODO Trial 2: Register middleware here.

                IStorage dataStore = new MemoryStorage();
                options.State.Add(new ConversationState(dataStore));
            });

            // Create and register state accessors.
            // Accessors created here are passed into the IBot-derived class on every turn.
            services.AddSingleton<GameBotAccessors>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BotFrameworkOptions>>().Value;
                if (options == null)
                {
                    throw new InvalidOperationException("BotFrameworkOptions must be configured prior to setting up the state accessors");
                }

                var conversationState = options.State.OfType<ConversationState>().FirstOrDefault();
                if (conversationState == null)
                {
                    throw new InvalidOperationException("ConversationState must be defined and added before adding conversation-scoped state accessors.");
                }

                return new GameBotAccessors(conversationState)
                {
                    DialogStateAccessor = conversationState.CreateProperty<DialogState>(GameBotAccessors.DialogStateAccessorName),
                    GameScriptStateAccessor = conversationState.CreateProperty<IGameScriptState>(GameBotAccessors.GameScriptStateAccessorName)
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }

            app.UseStaticFiles();

            app.UseMvc();
            app.UseBotFramework();
        }
    }
}
