using System;
using System.Threading.Tasks;
using NServiceBus;
using Receiver;

public static class Program
{
    static async Task Main()
    {
        var endpointConfiguration = new NotificationServiceBusHost().GetEndpointConfiguration();

        var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop().ConfigureAwait(false);
    }
}