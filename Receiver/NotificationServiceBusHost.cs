using Microsoft.Data.SqlClient;
using NServiceBus;
using System;

namespace Receiver
{
    class NotificationServiceBusHost
    {
        public EndpointConfiguration GetEndpointConfiguration()
        {
            var receiverEndpointName = "ReceiverEndpoint";
            var connectionString = @"Data Source=127.0.0.1,1433;Database=NsbSamplesSql;User Id=sa;Password=Lineoftd1;Max Pool Size=100";

            var defaultSchemaName = "dbo";
            var auditQueueName = "audit";

            SqlHelper.EnsureDatabaseExists(connectionString);

            var endpointConfiguration = new EndpointConfiguration(receiverEndpointName);

            endpointConfiguration.AuditProcessedMessagesTo(auditQueueName);

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate =>
                {
                //default is 5, immediate retries also can be disabled by setting to 0
                immediate.NumberOfRetries(2);
                });

            recoverability.Delayed(
                delayed =>
                {
                //delayed retries also can be disabled by setting to 0
                delayed.NumberOfRetries(1);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(10));
                });

            recoverability.AddUnrecoverableException<ArgumentException>();

            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(connectionString);
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema(defaultSchemaName);
            persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
            persistence.TablePrefix("");

            return endpointConfiguration;
        }
    }
}
