using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Easy.Transfers.CrossCutting.Configuration.Extensions
{
    public static class JsonExtensions
    {
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Converters = new JsonConverter[] { new StringEnumConverter() }
        };

        public static byte[] ToJsonBytes(this object source)
        {
            if (source == null)
                return null;
            var instring = JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
            return Utf8NoBom.GetBytes(instring);
        }

        public static string ToJson(this object source)
        {
            if (source == null) return null;
            var instring = JsonConvert.SerializeObject(source, Formatting.Indented, JsonSettings);
            return instring;
        }

      
        public static string ToJson(this object source, JsonSerializerSettings jsonSettings)
        {
            if (source == null) return null;
            if (jsonSettings == null) throw new ArgumentNullException(nameof(jsonSettings));
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var instring = JsonConvert.SerializeObject(source, Formatting.Indented, jsonSettings);
            return instring;
        }

        public static string ToCanonicalJson(this object source)
        {
            if (source == null)
                return null;
            var instring = JsonConvert.SerializeObject(source, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return instring;
        }

        public static T ParseJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            var result = JsonConvert.DeserializeObject<T>(json, JsonSettings);
            return result;
        }

        public static T ParseJson<T>(this string json, JsonSerializerSettings settings)
        {
            if (string.IsNullOrEmpty(json)) return default(T);
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            return result;
        }

        public static T ParseJson<T>(this byte[] json)
        {
            if (json == null || json.Length == 0) return default(T);
            var result = JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), JsonSettings);
            return result;
        }

        public static T ParseJson<T>(this byte[] json, JsonSerializerSettings settings)
        {
            if (json == null || json.Length == 0) return default(T);
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var result = JsonConvert.DeserializeObject<T>(Utf8NoBom.GetString(json), settings);
            return result;
        }

        public static object JsonDeserializeObject(JObject value, Type type, JsonSerializerSettings settings)
        {
            var jsonSerializer = JsonSerializer.Create(settings);
            return jsonSerializer.Deserialize(new JTokenReader(value), type);
        }
    }
}
