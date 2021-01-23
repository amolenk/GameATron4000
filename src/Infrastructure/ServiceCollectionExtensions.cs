using System;
using GameATron4000.Core.Messages;
using GameATron4000.Core.Services;
using GameATron4000.Infrastructure.Lua;
using GameATron4000.Infrastructure.Mediator;
using GameATron4000.Infrastructure.Phaser;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extensions for registering application services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            AddMediator(services);
            AddGameScript(services);
            AddGraphicsEngine(services);
        }

        public static void AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator, MediatorImpl>();
        }

        public static void AddGameScript(this IServiceCollection services)
        {
            services.AddScoped<IGameScript>(sp => new LuaGameScript(sp.GetRequiredService<IMediator>()/*, sp.GetService<GameInfo>().Name*/));
            //services.AddScoped<IMessageHandler<BeforeRoomEnteredEvent>>(sp => sp.GetService<LuaGameScript>());
            //services.AddScoped<IMessageHandler<AfterRoomEnteredEvent>>(sp => sp.GetService<LuaGameScript>());
        }

        public static void AddGraphicsEngine(this IServiceCollection services)
        {
            services.AddScoped<IGraphicsEngine, PhaserGraphicsEngine>();
        }
    }
}
