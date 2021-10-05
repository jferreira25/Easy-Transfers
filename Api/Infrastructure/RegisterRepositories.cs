using Easy.Transfers.Admin.Core;
using Easy.Transfers.Domain.Interfaces.MongoDb;
using Easy.Transfers.Infrastructure.Data.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterRepositories : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(ITransactionRepository), typeof(TransactionRepository));
        }
    }
}
