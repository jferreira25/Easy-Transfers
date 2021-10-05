using Easy.Transfers.Admin.Core;
using Easy.Transfers.Domain.Commands.FundTransfer.Create;
using Easy.Transfers.Domain.Commands.FundTransfer.CreateTransaction;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterMediator : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(
                typeof(CreateFundTransferCommand).GetTypeInfo().Assembly,
                typeof(CreateTransactionFundTransferCommand).GetTypeInfo().Assembly);
        }
    }
}
