using AutoFixture;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Infrastructure.Data.MongoDb;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Easy.Transfers.Tests.Unity.Mongo
{
    public class BaseRepositoryTest
    {
        public readonly Fixture _fixture;
        public BaseRepositoryTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void MongoDBContext_Constructor_Success()
        {
            Mock<IMongoClient> mockMongoClient = _fixture.Freeze<Mock<IMongoClient>>();
            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(new Mock<IMongoDatabase>().Object);

            var contexto = new BaseRepository<Transaction>(mockMongoClient.Object);

            Assert.NotNull(contexto);
        }

        [Fact]
        public void MongoDBContext_Should_GetCollection_Fail_EmptyName()
        {
            var mockMongoClient = _fixture.Freeze<Mock<IMongoClient>>();

            mockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(new Mock<IMongoDatabase>().Object);

            var contexto = new BaseRepository<Transaction>(mockMongoClient.Object);

            var collection = contexto.Collection;

            Assert.Null(collection);
        }
    }
}
