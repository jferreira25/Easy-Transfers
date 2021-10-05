using Easy.Transfers.CrossCutting.Configuration;
using Easy.Transfers.CrossCutting.Configuration.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Easy.Transfers.Infrastructure.Data.MongoDb
{
    public class BaseRepository<T>
    {
        private readonly string _database;
        private readonly IMongoClient _mongoClient;

        private readonly string _collection = typeof(T).Name;

        public BaseRepository(IMongoClient mongoClient)
        {
            _database = AppSettings.Settings.MongoConnections.DataBaseName;
            
            (_mongoClient) = (mongoClient);

            BsonSerializer.RegisterSerializationProvider(new LocalDateTimeSerializationProvider());
        }

        public IMongoCollection<T> Collection =>
         _mongoClient.GetDatabase(_database).GetCollection<T>(_collection);

    }
}
