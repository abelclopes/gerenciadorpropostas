using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace API.Messaging
{
    public class RabbitMqIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqIntegrationEventPublisher> _logger;

        public RabbitMqIntegrationEventPublisher(RabbitMqOptions options, ILogger<RabbitMqIntegrationEventPublisher> logger)
        {
            _options = options;
            _logger = logger;
        }

        public Task PublishAsync(string routingKey, string payload, CancellationToken cancellationToken = default)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: _options.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
            );

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var body = Encoding.UTF8.GetBytes(payload);
            channel.BasicPublish(_options.Exchange, routingKey, properties, body);

            _logger.LogInformation("Published integration event RoutingKey={RoutingKey} Exchange={Exchange}", routingKey, _options.Exchange);
            return Task.CompletedTask;
        }
    }
}
