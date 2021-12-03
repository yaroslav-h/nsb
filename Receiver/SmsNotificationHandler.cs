using System;
using System.Threading.Tasks;
using NServiceBus;

public class SmsNotificationHandler :
    IHandleMessages<SendSms>
{
    public Task Handle(SendSms message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Sending a message to an SMS API.");

        //throw new ArgumentException("handler exception");

        var messageAccepted = new SendNotificationResponse
        {
            PaymentId = message.PaymentId,
            Amount = message.Amount,
            DateTime = message.DateTime
        };

        return context.Reply(messageAccepted);
    }
}