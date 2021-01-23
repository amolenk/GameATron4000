using System;
using GameATron4000.Core.Domain;
using GameATron4000.Core.Messages;
using GameATron4000.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extensions for registering application services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddDomain(this IServiceCollection services)
        {
            services.AddScoped<World>();
            services.AddScoped<IMessageHandler<ChangeCameraFocusCommand>>(sp => sp.GetService<World>());
        }
    }
}
