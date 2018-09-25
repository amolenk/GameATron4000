using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.Core.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.Mvc;

namespace GameATron4000
{
    public class Startup
    {
        // public Startup(IHostingEnvironment env)
        // {
        //     var builder = new ConfigurationBuilder()
        //         .SetBasePath(env.ContentRootPath)
        //         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //         .AddEnvironmentVariables();

        //     Configuration = builder.Build();
        // }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddBot<GameBot>(options =>
            // {
            //     options.CredentialProvider = new ConfigurationCredentialProvider(Configuration);

            //     IStorage dataStore = new MemoryStorage();
            //     StateSettings settings = new StateSettings();

            //     options.Middleware.Add(new ConversationState<Dictionary<string, object>>(dataStore, settings));
            // });

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

//            app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            // app.UseBotFramework();
        }
    }
}
