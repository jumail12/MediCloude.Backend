using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Infrastructure.Consumers.Specialization;
using BusinessService.Infrastructure.Persistance;
using BusinessService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Infrastructure.Configurations_
{
    public static class BusinessServiceRegistration
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services)
        {
            var conString = Environment.GetEnvironmentVariable("DBB_CONNECTION");

            services.AddDbContext<BusinessDbContext>(options =>
                options.UseNpgsql(conString, b =>
                    b.MigrationsAssembly("BusinessService.Infrastructure")
                    .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)
                ));
        }

        public static void RepoConfiguration(this IServiceCollection services)
        {
                services.AddScoped<ISpecializationRepo,SpecializationRepo>();
        }

        public static void ServiceConfiguration(this IServiceCollection services)
        {

        }

        //request rabbitmq
        public static void RabbitMqRequestConfig(this IServiceCollection services)
        {

        }

        //reponse rabbitmq
        public static void RabbitMqResponseConfig(this IServiceCollection services)
        {

            var host= Environment.GetEnvironmentVariable("RabbitMqHostB");
            var username=Environment.GetEnvironmentVariable("RabbitMqUsernameB");
            var password = Environment.GetEnvironmentVariable("RabbitMqPasswordB");

            //spelization esits response
            services.AddMassTransit(config =>
            {
                config.AddConsumer<SpecializationConsumer>(); //register consumer
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("spl-exists-queue", e =>
                    {
                        e.ConfigureConsumer<SpecializationConsumer>(ctx);  //bind to queue
                    });
                });
            });
        }
    }
}
