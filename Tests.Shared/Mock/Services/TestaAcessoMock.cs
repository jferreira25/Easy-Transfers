using AutoFixture;
using Easy.Transfers.Domain.Interfaces.Services;
using Easy.Transfers.Domain.Services;
using Easy.Transfers.Tests.Shared.Core;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Transfers.Tests.Shared.Mock.Services
{
    public class TestaAcessoMock : BaseMock<ITestaAcesso>
    {
        public readonly Fixture _fixture;
        public  Account _account;
        public Account _account1;

        public TestaAcessoMock()
        {
            _fixture = new Fixture();

            _account = _fixture.Create<Account>();
            _account1 = _fixture.Create<Account>();
        }
        public override Mock<ITestaAcesso> GetDefaultInstance()
        {
             TestaAcesso();
            return Mock;
        }

        public Mock<ITestaAcesso> GetDefaultInstanceByAccountNumber()
        {
            _account.AccountNumber = "2345";
            _account1.AccountNumber = "6789";

            Mock.SetupSequence(r => r.GetAccountByAccountNumber(It.IsAny<string>())).ReturnsAsync(_account).ReturnsAsync(_account1);
            Setup(r => r.TransferAsync(It.IsAny<TransferAccount>()), Task.FromResult(true));
            
            return Mock;
        }


        private void TestaAcesso()
        {
            Setup(r => r.GetAccountByAccountNumber(It.IsAny<string>()), _account);
            Setup(r => r.TransferAsync(It.IsAny<TransferAccount>()), Task.FromResult(true));
        }
    }
}
