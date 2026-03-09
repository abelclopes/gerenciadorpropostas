using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace API.Messaging
{
    public static class OutboxSchemaBootstrapper
    {
        public static void EnsureCreated(DatabaseFacade database)
        {
            var sql = @"
IF OBJECT_ID(N'dbo.OutboxMessages', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OutboxMessages (
        Id uniqueidentifier NOT NULL PRIMARY KEY,
        DataCriacao datetime2 NOT NULL,
        DataAtualizacao datetime2 NULL,
        Excluido bit NOT NULL DEFAULT(0),
        [Type] nvarchar(200) NOT NULL,
        RoutingKey nvarchar(200) NOT NULL,
        Payload nvarchar(max) NOT NULL,
        ProcessedAt datetime2 NULL,
        Attempts int NOT NULL DEFAULT(0),
        LastError nvarchar(4000) NULL
    );
    CREATE INDEX IX_OutboxMessages_ProcessedAt ON dbo.OutboxMessages (ProcessedAt);
END";

            database.ExecuteSqlRaw(sql);
        }
    }
}
