using Newtonsoft.Json;

namespace Easy.Transfers.CrossCutting.Configuration.AppModels
{
    public class MongoConnection
    {
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("dataBaseName")]
        public string DataBaseName { get; set; }

    }
}
