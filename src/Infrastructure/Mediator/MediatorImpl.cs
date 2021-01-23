using System;
using System.Threading.Tasks;
using GameATron4000.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameATron4000.Infrastructure.Mediator
{
    /// <summary>
    /// Implements the Mediator pattern by routing commands and events to the given handlers.
    /// </summary>
    public class MediatorImpl : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public MediatorImpl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<T>(T @event) where T : class
        {
            var handlers = _serviceProvider.GetServices<IAsyncMessageHandler<T>>();

            foreach (var handler in handlers)
            {
                await handler.Handle(@event);
            }
        }

        public void Publish<T>(T @event) where T : class
        {
            var handlers = _serviceProvider.GetServices<IMessageHandler<T>>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }

        public async Task SendAsync<T>(T command) where T : class
        {
            var handler = _serviceProvider.GetService<IAsyncMessageHandler<T>>();

            await handler.Handle(command);
        }

        public void Send<T>(T command) where T : class
        {
            var handler = _serviceProvider.GetService<IMessageHandler<T>>();

            handler.Handle(command);
        }
    }
}