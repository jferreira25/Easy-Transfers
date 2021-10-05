using Newtonsoft.Json;

namespace Easy.Transfers.CrossCutting.Configuration.AppModels
{
    public class RabbitMqConnection
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
        [JsonProperty("queueName")]
        public string QueueName { get; set; }

    }
}
