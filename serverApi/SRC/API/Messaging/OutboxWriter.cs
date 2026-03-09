using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DOMAIN;
using DOMAIN.Interfaces;

namespace API.Messaging
{
    public class OutboxWriter : IOutboxWriter
    {
        private readonly IContext _context;

        public OutboxWriter(IContext context)
        {
            _context = context;
        }

        public Task EnqueueAsync(string messageType, string routingKey, object payload, CancellationToken cancellationToken = default)
        {
            var message = new OutboxMessage
            {
                Type = messageType,
                RoutingKey = routingKey,
                Payload = JsonSerializer.Serialize(payload),
                Attempts = 0
            };

            _context.OutboxMessages.Add(message);
            return Task.CompletedTask;
        }
    }
}
