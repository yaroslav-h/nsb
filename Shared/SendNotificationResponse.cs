using NServiceBus;
using System;

public class SendNotificationResponse : IMessage
{
    public Guid MessageId { get; set; }
}