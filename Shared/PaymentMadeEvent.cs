using NServiceBus;
using System;

public class PaymentMadeEvent :
    IEvent
{
    public int PaymentId { get; set; }

    public int Amount { get; set; }

    public DateTime DateTime { get; set; }
}