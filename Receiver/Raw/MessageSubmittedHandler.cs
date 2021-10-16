using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Raw
{
    public class MessageSubmittedHandler :
        IHandleMessages<SendSms>
    {
        static ILog log = LogManager.GetLogger<MessageSubmittedHandler>();

        public async Task Handle(SendSms message, IMessageHandlerContext context)
        {
            log.Info($"Message {message.MessageId} worth {message.Value} persisted by raw sql");

            #region StoreDataRaw

            var session = context.SynchronizedStorageSession.SqlPersistenceSession();

            var sql = @"insert into receiver.SubmittedMessage
                                    (Id, Value)
                        values      (@Id, @Value)";
            using (var command = new SqlCommand(
                cmdText: sql,
                connection: (SqlConnection) session.Connection,
                transaction: (SqlTransaction) session.Transaction))
            {
                var parameters = command.Parameters;
                parameters.AddWithValue("Id", $"Raw-{message.MessageId}");
                parameters.AddWithValue("Value", message.Value);
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }

            #endregion
        }
    }
}