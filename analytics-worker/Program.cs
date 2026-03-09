using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AnalyticsWorkerService>();

await builder.Build().RunAsync();

public sealed class AnalyticsWorkerService : BackgroundService
{
    private readonly ILogger<AnalyticsWorkerService> _logger;
    private readonly AnalyticsWorkerOptions _options;

    public AnalyticsWorkerService(ILogger<AnalyticsWorkerService> logger)
    {
        _logger = logger;
        _options = AnalyticsWorkerOptions.FromEnvironment();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Analytics worker started. Queue={Queue} RoutingKey={RoutingKey}", _options.QueueName, _options.RoutingKey);
        await EnsureSchemaAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeLoopAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Analytics consumer loop failed. Retrying in 5s.");
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
                var evt = Parse(payload);
                if (evt == null)
                {
                    _logger.LogWarning("Invalid analytics payload. Dropping event.");
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                    return;
                }

                await ApplyProjectionAsync(evt, ea.RoutingKey, payload, stoppingToken);
                channel.BasicAck(ea.DeliveryTag, multiple: false);
                _logger.LogInformation("Analytics projection updated. PropostaId={PropostaId}", evt.PropostaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process analytics event. RoutingKey={RoutingKey}", ea.RoutingKey);
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        channel.BasicConsume(_options.QueueName, autoAck: false, consumer: consumer);
        _logger.LogInformation("Analytics consuming from RabbitMQ at {Host}:{Port}", _options.RabbitHost, _options.RabbitPort);

        while (!stoppingToken.IsCancellationRequested && connection.IsOpen)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private static PropostaCriadaEvent? Parse(string payload)
    {
        using var doc = JsonDocument.Parse(payload);
        var root = doc.RootElement;

        if (!root.TryGetProperty("propostaId", out var propostaIdNode))
        {
            return null;
        }

        if (!Guid.TryParse(propostaIdNode.GetString(), out var propostaId))
        {
            return null;
        }

        var status = root.TryGetProperty("status", out var statusNode) && statusNode.TryGetInt32(out var parsedStatus)
            ? parsedStatus
            : 0;

        var valorString = root.TryGetProperty("valor", out var valorNode)
            ? valorNode.GetString() ?? "0"
            : "0";

        if (!decimal.TryParse(valorString, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
        {
            valor = 0m;
        }

        return new PropostaCriadaEvent(propostaId, status, valor);
    }

    private async Task EnsureSchemaAsync(CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_options.SqlConnectionString);
        await conn.OpenAsync(cancellationToken);

        var sql = """
                  IF OBJECT_ID(N'dbo.AnalyticsEventInbox', N'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.AnalyticsEventInbox (
                          EventId uniqueidentifier NOT NULL PRIMARY KEY,
                          ReceivedAtUtc datetime2 NOT NULL,
                          RoutingKey nvarchar(200) NOT NULL,
                          Payload nvarchar(max) NOT NULL
                      );
                      CREATE INDEX IX_AnalyticsEventInbox_ReceivedAtUtc ON dbo.AnalyticsEventInbox (ReceivedAtUtc);
                  END;

                  IF OBJECT_ID(N'dbo.DashboardKpiSnapshot', N'U') IS NULL
                  BEGIN
                      CREATE TABLE dbo.DashboardKpiSnapshot (
                          Id int NOT NULL PRIMARY KEY,
                          TotalPropostas int NOT NULL,
                          PropostasAguardando int NOT NULL,
                          PropostasAprovadas int NOT NULL,
                          PropostasOutrosStatus int NOT NULL,
                          ValorTotal decimal(18,2) NOT NULL,
                          UpdatedAtUtc datetime2 NOT NULL
                      );
                      INSERT INTO dbo.DashboardKpiSnapshot
                      (Id, TotalPropostas, PropostasAguardando, PropostasAprovadas, PropostasOutrosStatus, ValorTotal, UpdatedAtUtc)
                      VALUES (1,0,0,0,0,0,SYSUTCDATETIME());
                  END
                  """;

        await using var cmd = new SqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task ApplyProjectionAsync(PropostaCriadaEvent evt, string routingKey, string payload, CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_options.SqlConnectionString);
        await conn.OpenAsync(cancellationToken);
        await using var tx = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            // Idempotencia: nao processa o mesmo evento duas vezes.
            await using (var inboxInsert = conn.CreateCommand())
            {
                inboxInsert.Transaction = (SqlTransaction)tx;
                inboxInsert.CommandText = """
                                          IF NOT EXISTS (SELECT 1 FROM dbo.AnalyticsEventInbox WHERE EventId=@EventId)
                                          BEGIN
                                              INSERT INTO dbo.AnalyticsEventInbox (EventId, ReceivedAtUtc, RoutingKey, Payload)
                                              VALUES (@EventId, SYSUTCDATETIME(), @RoutingKey, @Payload);
                                              SELECT 1;
                                          END
                                          ELSE
                                          BEGIN
                                              SELECT 0;
                                          END
                                          """;
                inboxInsert.Parameters.AddWithValue("@EventId", evt.PropostaId);
                inboxInsert.Parameters.AddWithValue("@RoutingKey", routingKey);
                inboxInsert.Parameters.AddWithValue("@Payload", payload);
                var shouldApply = (int)(await inboxInsert.ExecuteScalarAsync(cancellationToken) ?? 0);

                if (shouldApply == 0)
                {
                    await tx.CommitAsync(cancellationToken);
                    return;
                }
            }

            await using (var updateSnapshot = conn.CreateCommand())
            {
                updateSnapshot.Transaction = (SqlTransaction)tx;
                updateSnapshot.CommandText = """
                                             UPDATE dbo.DashboardKpiSnapshot
                                             SET
                                               TotalPropostas = TotalPropostas + 1,
                                               PropostasAguardando = PropostasAguardando + CASE WHEN @Status = 1 THEN 1 ELSE 0 END,
                                               PropostasAprovadas = PropostasAprovadas + CASE WHEN @Status = 2 THEN 1 ELSE 0 END,
                                               PropostasOutrosStatus = PropostasOutrosStatus + CASE WHEN @Status NOT IN (1,2) THEN 1 ELSE 0 END,
                                               ValorTotal = ValorTotal + @Valor,
                                               UpdatedAtUtc = SYSUTCDATETIME()
                                             WHERE Id = 1;
                                             """;
                updateSnapshot.Parameters.AddWithValue("@Status", evt.Status);
                updateSnapshot.Parameters.AddWithValue("@Valor", evt.Valor);
                await updateSnapshot.ExecuteNonQueryAsync(cancellationToken);
            }

            await tx.CommitAsync(cancellationToken);
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

public sealed record PropostaCriadaEvent(Guid PropostaId, int Status, decimal Valor);

public sealed class AnalyticsWorkerOptions
{
    public required string RabbitHost { get; init; }
    public required int RabbitPort { get; init; }
    public required string RabbitUser { get; init; }
    public required string RabbitPassword { get; init; }
    public required string Exchange { get; init; }
    public required string QueueName { get; init; }
    public required string RoutingKey { get; init; }
    public required string SqlConnectionString { get; init; }

    public static AnalyticsWorkerOptions FromEnvironment()
    {
        return new AnalyticsWorkerOptions
        {
            RabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
            RabbitPort = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : 5672,
            RabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            RabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
            Exchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE") ?? "gerenciadorpropostas.events",
            QueueName = Environment.GetEnvironmentVariable("ANALYTICS_QUEUE_NAME") ?? "analytics.proposta.criada",
            RoutingKey = Environment.GetEnvironmentVariable("ANALYTICS_ROUTING_KEY") ?? "proposta.criada",
            SqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                ?? "Server=db;Database=gerenciadorPropostas;User Id=sa;Password=Dqmajjdr021221;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true"
        };
    }
}
