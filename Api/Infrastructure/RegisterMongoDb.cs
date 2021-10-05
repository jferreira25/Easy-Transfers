using Easy.Transfers.Admin.Core;
using Easy.Transfers.CrossCutting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterMongoDb : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(AppSettings.Settings.MongoConnections.ConnectionString);
            });

            services.AddScoped(c =>
                c.GetService<IMongoClient>().StartSession());
        }
    }
}
