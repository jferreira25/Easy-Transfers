using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Subscriber.Interface
{
    public interface IRabbitMqConsumer
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
    }
}
