
using Easy.Transfers.Domain.Entities.Mongo;
using System.Threading.Tasks;

namespace Easy.Transfers.Domain.Interfaces.MongoDb
{
    public interface ITransactionRepository
    {
        Task CreateAsync(Transaction entity);
        Task<Transaction> GetByTransactionIdAsync(string transactionId);
        Task<Transaction> UpdateAsync(Transaction entity);
    }
}
