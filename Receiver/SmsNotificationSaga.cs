using System;
using System.Threading.Tasks;
using NServiceBus;

public class SmsNotificationSaga :
    Saga<SmsNotificationSaga.SagaData>,
    IAmStartedByMessages<PaymentMadeEvent>,
    IHandleMessages<SendNotificationResponse>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.ConfigureMapping<PaymentMadeEvent>(msg => msg.PaymentId).ToSaga(saga => saga.PaymentId);
    }

    public async Task Handle(PaymentMadeEvent message, IMessageHandlerContext context)
    {
        Data.PaymentId = message.PaymentId;

        //throw new ArgumentException("saga exception");

        var messageSubmitted = new SendSms
        {
            PaymentId = message.PaymentId,
            Amount = message.Amount,
            DateTime = message.DateTime
        };

        await context.SendLocal(messageSubmitted);
    }

    public async Task Handle(SendNotificationResponse message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Writing logs for PaymentId: {Data.PaymentId} accepted.");
    }

    public class SagaData :
        ContainSagaData
    {
        public int PaymentId { get; set; }
    }
}