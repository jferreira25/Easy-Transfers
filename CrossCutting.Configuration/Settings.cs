using Newtonsoft.Json;

namespace Easy.Transfers.CrossCutting.Configuration
{
    public class Settings
    {
        [JsonProperty(PropertyName = "applicationName")]
        public string ApplicationName { get; set; }
    }
}
