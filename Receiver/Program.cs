using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sql.Receiver";

        #region ReceiverConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Receiver");

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            immediate =>
            {
                //default is 5, immediate retries also can be disabled by setting to 0
                immediate.NumberOfRetries(2);
            });

        //recoverability.Delayed(
        //    delayed =>
        //    {
        //        //delayed retries also can be disabled by setting to 0
        //        delayed.NumberOfRetries(2);
        //        delayed.TimeIncrease(TimeSpan.FromMinutes(5));
        //    });

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
        var connection = @"Data Source=127.0.0.1,1433;Database=NsbSamplesSql;User Id=sa;Password=Lineoftd1;Max Pool Size=100";

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("receiver");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");
        transport.UseSchemaForQueue("Samples.Sql.Sender", "sender");
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        var subscriptions = transport.SubscriptionSettings();
        subscriptions.CacheSubscriptionInformationFor(TimeSpan.FromMinutes(1));
        subscriptions.SubscriptionTableName(tableName: "Subscriptions", schemaName: "dbo");

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(SendNotificationResponse), "Samples.Sql.Sender");

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("receiver");
        persistence.ConnectionBuilder(() => new SqlConnection(connection));
        persistence.TablePrefix("");

        #endregion

        SqlHelper.CreateSchema(connection, "receiver");
        var allText = File.ReadAllText("Startup.sql");
        SqlHelper.ExecuteSql(connection, allText);
        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop().ConfigureAwait(false);
    }
}