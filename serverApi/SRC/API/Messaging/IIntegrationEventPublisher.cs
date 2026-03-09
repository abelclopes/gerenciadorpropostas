using System.Threading;
using System.Threading.Tasks;

namespace API.Messaging
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync(string routingKey, string payload, CancellationToken cancellationToken = default);
    }
}
