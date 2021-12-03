using System;
using System.Threading.Tasks;
using NServiceBus;
using Sender;

public static class Program
{
    static Random random;

    public static async Task Main()
    {
        random = new Random();

        var endpointConfiguration = new NotificationServiceBusHost().GetEndpointConfiguration();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press enter to publish an event; Or any other key to exit...");

        var key = ConsoleKey.Enter;
        while (key == ConsoleKey.Enter)
        {
            key = Console.ReadKey().Key;
            Console.WriteLine();

            var paymentMade = new PaymentMadeEvent
            {
                PaymentId = random.Next(1, 100),
                Amount = random.Next(1, 1000),
                DateTime = DateTime.Now
            };

            await endpointInstance.Publish(paymentMade)
                .ConfigureAwait(false);

            Console.WriteLine("Published PaymentMade event");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}