using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationWorkerService>();

await builder.Build().RunAsync();

public sealed class NotificationWorkerService : BackgroundService
{
    private readonly ILogger<NotificationWorkerService> _logger;
    private readonly WorkerOptions _options;

    public NotificationWorkerService(ILogger<NotificationWorkerService> logger)
    {
        _logger = logger;
        _options = WorkerOptions.FromEnvironment();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification worker started. Queue={Queue} RoutingKey={RoutingKey}", _options.QueueName, _options.RoutingKey);
        await EnsureSchemaAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeLoopAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Consumer loop failed. Retrying in 5s.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ConsumeLoopAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.RabbitHost,
            Port = _options.RabbitPort,
            UserName = _options.RabbitUser,
            Password = _options.RabbitPassword,
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(_options.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);
        channel.QueueDeclare(_options.QueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(_options.QueueName, _options.Exchange, _options.RoutingKey);
        channel.BasicQos(0, 20, global: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (_, ea) =>
        {
            var payload = Encoding.UTF8.GetString(ea.Body.ToArray());

            try
            {
                await SaveNotificationAsync(ea.RoutingKey, payload, stoppingToken);
                channel.BasicAck(ea.DeliveryTag, multiple: false);
                _logger.LogInformation("Notification event processed. RoutingKey={RoutingKey}", ea.RoutingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process notification event. RoutingKey={RoutingKey}", ea.RoutingKey);
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        channel.BasicConsume(_options.QueueName, autoAck: false, consumer: consumer);

        _logger.LogInformation("Consuming from RabbitMQ at {Host}:{Port}", _options.RabbitHost, _options.RabbitPort);

        while (!stoppingToken.IsCancellationRequested && connection.IsOpen)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task EnsureSchemaAsync(CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_options.SqlConnectionString);
        await conn.OpenAsync(cancellationToken);

        var sql = """
                  IF OBJECT_ID(N'dbo.NotificationEvents', N'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.NotificationEvents (
                          Id uniqueidentifier NOT NULL PRIMARY KEY,
                          CreatedAtUtc datetime2 NOT NULL,
                          RoutingKey nvarchar(200) NOT NULL,
                          Payload nvarchar(max) NOT NULL
                      );
                      CREATE INDEX IX_NotificationEvents_CreatedAtUtc ON dbo.NotificationEvents (CreatedAtUtc);
                  END
                  """;

        await using var cmd = new SqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task SaveNotificationAsync(string routingKey, string payload, CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_options.SqlConnectionString);
        await conn.OpenAsync(cancellationToken);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = """
                          INSERT INTO dbo.NotificationEvents (Id, CreatedAtUtc, RoutingKey, Payload)
                          VALUES (@Id, SYSUTCDATETIME(), @RoutingKey, @Payload)
                          """;
        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@RoutingKey", routingKey);
        cmd.Parameters.AddWithValue("@Payload", payload);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}

public sealed class WorkerOptions
{
    public required string RabbitHost { get; init; }
    public required int RabbitPort { get; init; }
    public required string RabbitUser { get; init; }
    public required string RabbitPassword { get; init; }
    public required string Exchange { get; init; }
    public required string QueueName { get; init; }
    public required string RoutingKey { get; init; }
    public required string SqlConnectionString { get; init; }

    public static WorkerOptions FromEnvironment()
    {
        return new WorkerOptions
        {
            RabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
            RabbitPort = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : 5672,
            RabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            RabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
            Exchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE") ?? "gerenciadorpropostas.events",
            QueueName = Environment.GetEnvironmentVariable("NOTIFICATION_QUEUE_NAME") ?? "notifications.proposta.criada",
            RoutingKey = Environment.GetEnvironmentVariable("NOTIFICATION_ROUTING_KEY") ?? "proposta.criada",
            SqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                ?? "Server=db;Database=gerenciadorPropostas;User Id=sa;Password=Dqmajjdr021221;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true"
        };
    }
}
