using System.Threading;
using System.Threading.Tasks;

namespace API.Messaging
{
    public interface IOutboxWriter
    {
        Task EnqueueAsync(string messageType, string routingKey, object payload, CancellationToken cancellationToken = default);
    }
}
