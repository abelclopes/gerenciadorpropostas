using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace API.Messaging
{
    public class LoggingIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly ILogger<LoggingIntegrationEventPublisher> _logger;

        public LoggingIntegrationEventPublisher(ILogger<LoggingIntegrationEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(string routingKey, string payload, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("RabbitMQ disabled. Event not sent. RoutingKey={RoutingKey} Payload={Payload}", routingKey, payload);
            return Task.CompletedTask;
        }
    }
}
