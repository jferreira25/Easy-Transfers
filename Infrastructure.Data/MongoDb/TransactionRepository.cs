using Easy.Transfers.Domain.Interfaces.MongoDb;
using Easy.Transfers.Domain.Entities.Mongo;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Data.MongoDb
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoClient mongoClient) : base(mongoClient)
        {
        }

        public async Task CreateAsync(Transaction entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<Transaction> GetByTransactionIdAsync(string transactionId)
        {
            var filtro = Builders<Transaction>.Filter.Eq(x => x.TransactionId, transactionId);

            var resultado = await Collection.FindAsync(filtro).Result.FirstOrDefaultAsync();

            return resultado;
        }

        public async Task<Transaction> UpdateAsync(Transaction entity)
        {
            var resultado = await Collection.ReplaceOneAsync(
            doc => doc.Id == entity.Id,
            entity);

            if (resultado.ModifiedCount <= 0)
                return null;

            return entity;
        }
    }

}
