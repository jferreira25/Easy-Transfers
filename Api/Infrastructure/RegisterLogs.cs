using Easy.Transfers.Admin.Core;
using Easy.Transfers.CrossCutting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace Easy.Transfers.Admin.Infrastructure
{
    internal class RegisterLogs : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(AppSettings.Settings.ElasticConfiguration.Uri))
                {
                    AutoRegisterTemplate = true,
                }).CreateLogger();
        }
    }
}
