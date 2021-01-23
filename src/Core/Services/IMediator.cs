using System.Threading.Tasks;

namespace GameATron4000.Core.Services
{
    /// <summary>
    /// Classes that implement this interface allow indirect communication between objects. This recudes the
    /// dependencies between communicating objects, thereby reducing coupling.
    /// </summary>
    public interface IMediator
    {
        Task PublishAsync<T>(T @event) where T : class;

        Task SendAsync<T>(T command) where T : class;

        void Publish<T>(T @event) where T : class;

        void Send<T>(T command) where T : class;
    }
}
