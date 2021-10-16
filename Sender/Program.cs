using System;
using System.Threading.Tasks;
using NServiceBus;

public static class Program
{
    static Random random;

    public static async Task Main()
    {
        random = new Random();

        Console.Title = "Samples.Sql.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Sql.Sender");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        #region SenderConfiguration

        var connection = @"Data Source=127.0.0.1,1433;Database=NsbSamplesSql;User Id=sa;Password=Lineoftd1;Max Pool Size=100";
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connection);
        transport.DefaultSchema("sender");
        transport.UseSchemaForQueue("error", "dbo");
        transport.UseSchemaForQueue("audit", "dbo");

        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var subscriptions = transport.SubscriptionSettings();
        subscriptions.SubscriptionTableName(
            tableName: "Subscriptions", 
            schemaName: "dbo");

        #endregion

        SqlHelper.CreateSchema(connection, "sender");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }

            var submitMessage = new SendSmsNotification
            {
                MessageId = Guid.NewGuid(),
                Value = random.Next(100)
            };
            await endpointInstance.Publish(submitMessage)
                .ConfigureAwait(false);
            Console.WriteLine("Published SubmitMessage message");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}