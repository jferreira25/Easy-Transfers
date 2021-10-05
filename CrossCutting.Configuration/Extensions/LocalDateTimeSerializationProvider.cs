using MongoDB.Bson.Serialization;
using System;

namespace Easy.Transfers.CrossCutting.Configuration.Extensions
{
    public class LocalDateTimeSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(DateTime)) return new BsonUtcDateTimeSerializer();

            return null;
        }
    }
}
