using Easy.Transfers.CrossCutting.Configuration.AppModels;
using Newtonsoft.Json;

namespace Easy.Transfers.CrossCutting.Configuration
{
    public class AppSettings: Settings
    {
        public static AppSettings Settings => AppFileConfiguration<AppSettings>.Settings;

        [JsonProperty("mongoConnections")]
        public MongoConnection MongoConnections { get; set; }

        [JsonProperty("urlAccount")]
        public string UrlAccount { get; set; }

        [JsonProperty("rabbitMq")]
        public RabbitMqConnection RabbitMqConnection { get; set; }

        [JsonProperty("elasticConfiguration")]
        public ElasticConfiguration ElasticConfiguration { get; set; }
    }
}
