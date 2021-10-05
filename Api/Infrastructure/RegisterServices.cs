using Easy.Transfers.Admin.Core;
using Easy.Transfers.Domain.Interfaces.Services;
using Easy.Transfers.Infrastructure.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterServices : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(ITestaAcesso), typeof(TestaAcesso));
        }
    }
}
