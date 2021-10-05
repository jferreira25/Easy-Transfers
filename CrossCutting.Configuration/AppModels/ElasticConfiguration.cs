using Newtonsoft.Json;

namespace Easy.Transfers.CrossCutting.Configuration.AppModels
{
    public class ElasticConfiguration
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
