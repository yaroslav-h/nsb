using System.Threading.Tasks;
using NServiceBus;

public class SmsNotificationHandler :
    IHandleMessages<SendSms>
{
    public Task Handle(SendSms message, IMessageHandlerContext context)
    {
        var messageAccepted = new SendNotificationResponse
        {
            MessageId = message.MessageId
        };

        return context.Reply(messageAccepted);
    }
}