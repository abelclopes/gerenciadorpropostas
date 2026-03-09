using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Messaging
{
    public class OutboxDispatcherService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxDispatcherService> _logger;

        public OutboxDispatcherService(IServiceScopeFactory scopeFactory, ILogger<OutboxDispatcherService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox dispatcher started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DispatchPendingMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox dispatcher cycle failed");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task DispatchPendingMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IIntegrationEventPublisher>();

            var pending = await context.OutboxMessages
                .Where(x => x.ProcessedAt == null && x.Attempts < 10 && !x.Excluido)
                .OrderBy(x => x.DataCriacao)
                .Take(50)
                .ToListAsync(cancellationToken);

            if (!pending.Any())
            {
                return;
            }

            foreach (var message in pending)
            {
                try
                {
                    await publisher.PublishAsync(message.RoutingKey, message.Payload, cancellationToken);
                    message.ProcessedAt = DateTime.UtcNow;
                    message.LastError = null;
                }
                catch (Exception ex)
                {
                    message.Attempts += 1;
                    message.LastError = ex.Message.Length > 3900 ? ex.Message.Substring(0, 3900) : ex.Message;
                    _logger.LogWarning(ex, "Failed to publish outbox message Id={OutboxId} Attempt={Attempt}", message.Id, message.Attempts);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
