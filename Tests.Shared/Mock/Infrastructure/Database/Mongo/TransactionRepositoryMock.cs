using AutoFixture;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Domain.Interfaces.MongoDb;
using Easy.Transfers.Tests.Shared.Core;
using MongoDB.Bson;
using Moq;
using System.Threading.Tasks;

namespace Easy.Transfers.Tests.Shared.Mock.Infrastructure.Database.Mongo
{
    public class TransactionRepositoryMock : BaseMock<ITransactionRepository>
    {
        public readonly Fixture _fixture;
        public readonly Transaction _transaction;

        public TransactionRepositoryMock()
        {
            _fixture = new Fixture();

            _fixture.Register(ObjectId.GenerateNewId);

            _transaction = _fixture.Create<Transaction>();
        }
        public override Mock<ITransactionRepository> GetDefaultInstance()
        {
            TransactionRepository();
            return Mock;
        }

        private void TransactionRepository()
        {
            Setup(r => r.GetByTransactionIdAsync(It.IsAny<string>()), _transaction);
            Setup(r => r.CreateAsync(It.IsAny<Transaction>()), Task.FromResult(true));
        }
    }
}
