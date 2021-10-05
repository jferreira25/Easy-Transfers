using Easy.Transfers.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easy.Transfers.Domain.Interfaces.Services
{
    public interface ITestaAcesso
    {
        Task<List<Account>> GetAccounts();
        Task<Account> GetAccountByAccountNumber(string accountNumber);
        Task TransferAsync(TransferAccount transferAccount);
    }
}
