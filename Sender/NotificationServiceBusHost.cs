using NServiceBus;

namespace Sender
{
    class NotificationServiceBusHost
    {
        public EndpointConfiguration GetEndpointConfiguration()
        {
            var endpointName = "SenderEndpoint";
            var connectionString = @"Data Source=127.0.0.1,1433;Database=NsbSamplesSql;User Id=sa;Password=Lineoftd1;Max Pool Size=100";

            SqlHelper.EnsureDatabaseExists(connectionString);

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(connectionString);

            return endpointConfiguration;
        }
    }
}
