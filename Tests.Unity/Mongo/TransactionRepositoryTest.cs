using AutoFixture;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Infrastructure.Data.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static MongoDB.Driver.ReplaceOneResult;

namespace Easy.Transfers.Tests.Unity.Mongo
{
    public class TransactionRepositoryTest
    {
        public readonly Fixture _fixture;
        public readonly Transaction _entity;
        public readonly List<Transaction> _listEntity;

        public TransactionRepositoryTest()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);

            _entity = _fixture.Create<Transaction>();
            _listEntity = _fixture.Create<List<Transaction>>();
        }

        [Fact]
        public void  Must_InsertTransaction_When_Transaction_OK()
        {
            Mock<IMongoDatabase> mockMongoDataBase = _fixture.Freeze<Mock<IMongoDatabase>>();
            Mock<IMongoCollection<Transaction>> mockMongoCollection = new Mock<IMongoCollection<Transaction>>();

            var mockCursor = new Mock<IAsyncCursor<Transaction>>();
            mockCursor.Setup(_ => _.Current).Returns(_listEntity);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            mockMongoDataBase.Setup(x => x.GetCollection<Transaction>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mockMongoCollection.Object);

            Mock<IMongoClient> mockMongoClient = new Mock<IMongoClient>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockMongoDataBase.Object);

            var repository = new TransactionRepository(mockMongoClient.Object);

            var createTransaction = repository.CreateAsync(_entity);

            Assert.True(createTransaction.IsCompleted);
        }


        [Fact]
        public async Task Must_Return_TransactionByCorralationId()
        {
            Mock<IMongoDatabase> mockMongoDataBase = new Mock<IMongoDatabase>();
            Mock<IMongoCollection<Transaction>> mockMongoCollection = new Mock<IMongoCollection<Transaction>>();

            var mockCursor = new Mock<IAsyncCursor<Transaction>>();
            mockCursor.Setup(_ => _.Current).Returns(_listEntity);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            mockMongoCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Transaction>>(),
                                                       It.IsAny<FindOptions<Transaction, Transaction>>(),
                                                       It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);

            mockMongoDataBase.Setup(x => x.GetCollection<Transaction>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mockMongoCollection.Object);

            Mock<IMongoClient> mockMongoClient = new Mock<IMongoClient>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockMongoDataBase.Object);

            var repository = new TransactionRepository(mockMongoClient.Object);

            var existsTransaction = await repository.GetByTransactionIdAsync(_listEntity.FirstOrDefault().TransactionId);

            Assert.NotNull(existsTransaction);
        }

        [Fact]
        public async Task Must_Return_Null_When_Not_TransactionByCorralationId()
        {
            Mock<IMongoDatabase> mockMongoDataBase = new Mock<IMongoDatabase>();
            Mock<IMongoCollection<Transaction>> mockMongoCollection = new Mock<IMongoCollection<Transaction>>();

            var mockCursor = new Mock<IAsyncCursor<Transaction>>();
            mockCursor.Setup(_ => _.Current).Returns(_listEntity);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(false)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false))
                .Returns(Task.FromResult(false));

            mockMongoCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Transaction>>(),
                                                       It.IsAny<FindOptions<Transaction, Transaction>>(),
                                                       It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);

            mockMongoDataBase.Setup(x => x.GetCollection<Transaction>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mockMongoCollection.Object);

            Mock<IMongoClient> mockMongoClient = new Mock<IMongoClient>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockMongoDataBase.Object);

            var repository = new TransactionRepository(mockMongoClient.Object);

            var existeLandingPagePorDNS = await repository.GetByTransactionIdAsync(_entity.TransactionId);

            Assert.Null(existeLandingPagePorDNS);
        }

        [Fact]
        public async Task Must_UpdateCollection_When_exists() 
        {
            Mock<IMongoDatabase> mockMongoDataBase = new Mock<IMongoDatabase>();
            Mock<IMongoCollection<Transaction>> mockMongoCollection = new Mock<IMongoCollection<Transaction>>();

            var mockCursor = new Mock<IAsyncCursor<Transaction>>();
            mockCursor.Setup(_ => _.Current).Returns(_listEntity);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            var atualizarCollection = new Acknowledged(1, 1, 1);

            mockMongoCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Transaction>>(),
                                                             It.IsAny<Transaction>(), It.IsAny<ReplaceOptions>(),
                                                             It.IsAny<CancellationToken>())).ReturnsAsync(atualizarCollection);

            mockMongoDataBase.Setup(x => x.GetCollection<Transaction>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mockMongoCollection.Object);

            Mock<IMongoClient> mockMongoClient = new Mock<IMongoClient>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockMongoDataBase.Object);

            var repository = new TransactionRepository(mockMongoClient.Object);

            var landingPage = await repository.UpdateAsync(_entity);

            Assert.NotNull(landingPage);
        }

        [Fact]
        public async Task AtualizaCollectionExistenteRetornandoErro()
        {
            Mock<IMongoDatabase> mockMongoDataBase = new Mock<IMongoDatabase>();
            Mock<IMongoCollection<Transaction>> mockMongoCollection = new Mock<IMongoCollection<Transaction>>();

            var mockCursor = new Mock<IAsyncCursor<Transaction>>();
            mockCursor.Setup(_ => _.Current).Returns(_listEntity);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            var atualizarCollection = new Acknowledged(0, 0, 0);

            mockMongoCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Transaction>>(),
                                                             It.IsAny<Transaction>(), It.IsAny<ReplaceOptions>(),
                                                             It.IsAny<CancellationToken>())).ReturnsAsync(atualizarCollection);

            mockMongoDataBase.Setup(x => x.GetCollection<Transaction>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mockMongoCollection.Object);

            Mock<IMongoClient> mockMongoClient = new Mock<IMongoClient>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mockMongoDataBase.Object);

            var repository = new TransactionRepository( mockMongoClient.Object);

            var landingPage = await repository.UpdateAsync(_entity);

            Assert.Null(landingPage);
        }


    }
}
