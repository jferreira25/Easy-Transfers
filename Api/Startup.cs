using AutoMapper;
using Easy.Transfers.Admin.Extensions;
using Easy.Transfers.Admin.Filter;
using Easy.Transfers.Domain.Commands.FundTransfer.Create;
using Easy.Transfers.Infrastructure.Subscriber;
using Easy.Transfers.Infrastructure.Subscriber.Interface;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;

namespace Easy.Transfers
{
    public class Startup
    {
        public Container DependencyInjectionContainer { get; } = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            DependencyInjectionContainer.Options.DefaultLifestyle = Lifestyle.Scoped;
            DependencyInjectionContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseSimpleInjectorAspNetRequestScoping(DependencyInjectionContainer);

            services
                .AddMvc(options => options.Filters.Add(typeof(ApiValidationFilter)))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateFundTransferCommand>());

            services.AddAutoMapper(typeof(TransactionConsumer), typeof(CreateFundTransferCommand));
            services.AddServicesInAssembly(Configuration);

            services.AddHttpClient();

            services.AddSingleton(typeof(IRabbitMqConsumer), typeof(TransactionConsumer));

            var sp = services.BuildServiceProvider();

            var rabbit = sp.GetService<IRabbitMqConsumer>();
            rabbit.RegisterOnMessageHandlerAndReceiveMessages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto base  V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
