using NServiceBus;

public class SendNotificationResponse : IMessage
{
    public int PaymentId { get; set; }
}