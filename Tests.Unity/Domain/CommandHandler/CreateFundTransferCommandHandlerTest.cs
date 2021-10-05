using AutoFixture;
using Easy.Transfers.Domain.Commands.FundTransfer.Create;
using Easy.Transfers.Domain.Publishers;
using Easy.Transfers.Infrastructure.Publisher;
using Easy.Transfers.Tests.Shared.Mock.Infrastructure.Database.Mongo;
using Easy.Transfers.Tests.Shared.Mock.Injection.Mappers;
using Easy.Transfers.Tests.Shared.Mock.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Easy.Transfers.Tests.Unity.Domain.CommandHandler
{
    public class CreateFundTransferCommandHandlerTest
    {
        public readonly Fixture _fixture;
        public readonly CreateFundTransferCommand _createFundTransferCommand;
       

        public CreateFundTransferCommandHandlerTest()
        {
            _fixture = new Fixture();

            _createFundTransferCommand = _fixture.Create<CreateFundTransferCommand>();
        }

        protected CreateFundTransferCommandHandler EstablishContext() => new CreateFundTransferCommandHandler(
              new TestaAcessoMock().GetDefaultInstance().Object,
              new TransactionRepositoryMock().GetDefaultInstance().Object,
              MappersMock.GetMock(),
              new Mock<TransactionPublisher<TransactionEvent>>().Object,
              new Mock<ILogger<CreateFundTransferCommandHandler>>().Object
       );

        [Fact]
        public void  Should_Transfer_Transaction_Fail_When_Accounts_Same_Values()
        {
            var command = new CreateFundTransferCommandHandler(
               new TestaAcessoMock().GetDefaultInstance().Object,
               new TransactionRepositoryMock().GetDefaultInstance().Object,
               MappersMock.GetMock(),
               new Mock<TransactionPublisher<TransactionEvent>>().Object,
               new Mock<ILogger<CreateFundTransferCommandHandler>>().Object);

            var response = EstablishContext().Handle(_createFundTransferCommand, CancellationToken.None);

            Assert.Equal("One or more errors occurred. (Not is posible to transfer same account)", response.Exception.Message);
        }

        [Fact]
        public async Task Should_Transfer_Transaction_Sucess()
        {
            var command = new CreateFundTransferCommandHandler(
               new TestaAcessoMock().GetDefaultInstanceByAccountNumber().Object,
               new TransactionRepositoryMock().GetDefaultInstance().Object,
               MappersMock.GetMock(),
               new Mock<TransactionPublisher<TransactionEvent>>().Object,
               new Mock<ILogger<CreateFundTransferCommandHandler>>().Object);

            var response = await command.Handle(_createFundTransferCommand, CancellationToken.None);

            Assert.NotNull(response);
        }
    }
}
