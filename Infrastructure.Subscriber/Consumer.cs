using Easy.Transfers.CrossCutting.Configuration;
using Easy.Transfers.Infrastructure.Subscriber.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Subscriber
{
    public class Consumer<T>: IRabbitMqConsumer
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        EventingBasicConsumer _consumer;
        public Consumer()
        {
            _factory = new ConnectionFactory() { HostName = AppSettings.Settings.RabbitMqConnection.Url, Port = AppSettings.Settings.RabbitMqConnection.Port };
            _factory.UserName = AppSettings.Settings.RabbitMqConnection.User;
            _factory.Password = AppSettings.Settings.RabbitMqConnection.Password;

            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();

            _channel.QueueDeclare(queue: AppSettings.Settings.RabbitMqConnection.QueueName,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
            _consumer = new EventingBasicConsumer(_channel);
        }

        public Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            _consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var myPayload = JsonConvert.DeserializeObject<T>(message);

                await ConsumeAsync(myPayload);
            };

            _channel.BasicConsume(queue: AppSettings.Settings.RabbitMqConnection.QueueName,
                                 autoAck: true,
                                 consumer: _consumer);
            
            return Task.CompletedTask;
        }

        public virtual Task ConsumeAsync(T obj)
        {
            return Task.FromResult(true);
        }
    }
}
