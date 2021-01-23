using System.Threading.Tasks;

namespace GameATron4000.Core.Services
{
    /// <summary>
    /// Classes that implement this interface can handle messages of type T.
    /// </summary>
    public interface IMessageHandler<T>
    {
        void Handle(T message);
    }
}