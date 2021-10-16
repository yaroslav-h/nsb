using System;
using NServiceBus;

public class SendSmsNotification :
    IEvent
{
    public Guid MessageId { get; set; }
    public int Value { get; set; }
}