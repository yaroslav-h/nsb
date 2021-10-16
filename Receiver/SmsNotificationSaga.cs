using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class SmsNotificationSaga :
    Saga<SmsNotificationSaga.SagaData>,
    IAmStartedByMessages<SendSmsNotification>,
    IHandleMessages<SendNotificationResponse>
{
    static ILog log = LogManager.GetLogger<SmsNotificationSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.ConfigureMapping<SendSmsNotification>(msg => msg.MessageId).ToSaga(saga => saga.MessageId);
    }

    public async Task Handle(SendSmsNotification message, IMessageHandlerContext context)
    {
        Data.MessageId = message.MessageId;

        var messageSubmitted = new SendSms
        {
            MessageId = message.MessageId,
            Value = message.Value
        };

        await context.SendLocal(messageSubmitted);
    }

    public async Task Handle(SendNotificationResponse message, IMessageHandlerContext context)
    {
        log.Info($"Message {message.MessageId} accepted.");
    }

    public class SagaData :
        ContainSagaData
    {
        public Guid MessageId { get; set; }
    }
}