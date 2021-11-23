using NServiceBus;

public class PaymentMadeEvent :
    IEvent
{
    public int PaymentId { get; set; }

    public int Amount { get; set; }
}