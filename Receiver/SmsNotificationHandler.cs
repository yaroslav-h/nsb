using System.Threading.Tasks;
using NServiceBus;

public class SmsNotificationHandler :
    IHandleMessages<SendSms>
{
    public Task Handle(SendSms message, IMessageHandlerContext context)
    {
        var messageAccepted = new SendNotificationResponse
        {
            PaymentId = message.PaymentId
        };

        return context.Reply(messageAccepted);
    }
}