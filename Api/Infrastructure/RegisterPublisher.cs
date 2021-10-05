using Easy.Transfers.Admin.Core;
using Easy.Transfers.Domain.Publishers;
using Easy.Transfers.Infrastructure.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterPublisher : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TransactionPublisher<TransactionEvent>>();
        }
    }
}
