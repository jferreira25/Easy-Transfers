using Easy.Transfers.CrossCutting.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Publisher
{
    public class TransactionPublisher<T>
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;

        public TransactionPublisher()
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
        }
        public Task<bool> SendMessage(T message)
        {
            var messageBody = JsonConvert.SerializeObject(message);

            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublish(exchange: "",
                                routingKey: AppSettings.Settings.RabbitMqConnection.QueueName,
                                basicProperties: null,
                                body: body);

            return Task.FromResult(true);
        }
    }
}

