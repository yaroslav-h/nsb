using NServiceBus;
using System;

public class SendNotificationResponse : IMessage
{
    public int PaymentId { get; set; }

    public int Amount { get; set; }

    public DateTime DateTime { get; set; }
}