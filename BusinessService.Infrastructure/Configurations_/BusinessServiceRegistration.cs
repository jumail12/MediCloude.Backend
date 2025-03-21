﻿using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Infrastructure.Consumers.Specialization;
using BusinessService.Infrastructure.Persistance;
using BusinessService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


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

     

        //rabbitmq
        public static void RabbitMqConfig(this IServiceCollection services)
        {

            var host= Environment.GetEnvironmentVariable("RabbitMqHostB");
            var username=Environment.GetEnvironmentVariable("RabbitMqUsernameB");
            var password = Environment.GetEnvironmentVariable("RabbitMqPasswordB");

            //spelization esits response
            services.AddMassTransit(config =>
            {
                config.AddConsumer<SpecializationConsumer>(); //register consumer for spl exists
                config.AddConsumer<GetAllSpecializationConsumer>(); //register consumer for geting all spls

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("spl-exists-queue", e =>
                    {
                        e.ConfigureConsumer<SpecializationConsumer>(ctx);  //bind to queue for  isExistspl
                    });

                    cfg.ReceiveEndpoint("spl-getall-queue", e =>
                    {
                        e.ConfigureConsumer<GetAllSpecializationConsumer>(ctx);  //bind to queue for getting all spls
                    });
                });
            });
        }
    }
}
