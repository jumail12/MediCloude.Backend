using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
using AuthService.Infrastructure.Consumers.Doctor;
using AuthService.Infrastructure.Consumers.Patient;
using AuthService.Infrastructure.Persistance;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Contarcts.Requests.Specialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace AuthService.Infrastructure.Configurations_
{
    public static class ServiceRegistration
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services)
        {
            var conString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(conString, b =>
                    b.MigrationsAssembly("AuthService.Infrastructure")
                    .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)
                ));
        }

        public static void RepoConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPatientRepo, PatientRepo>();
            services.AddScoped<IDrRepo,DrRepo>();
            services.AddScoped<IAdminRepo, AdminRepo>();
        }

        public static void ServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICommonService, CommonService>();
           
        }

        // rabbitmq
        public static void RabbitMqConfig(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("RabbitMqHostA");
            var username=Environment.GetEnvironmentVariable("RabbitMqUsernameA");
            var password = Environment.GetEnvironmentVariable("RabbitMqPasswordA");

          
            services.AddMassTransit(config =>
            {
                //dr
                config.AddConsumer<DrByIdConsumer>();  //register consumer for drby id
                config.AddConsumer<DrProfileUpdationConsumer>();
                config.AddConsumer<GetAllDrsConsumer>();
                config.AddConsumer<GetAllPatientConsumer>();
                config.AddConsumer<PatientBlockUnblockConsumer>();
                config.AddConsumer<DrBlockUnblockConsumer>();

                //patient
                config.AddConsumer<PatientByIdConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint("dr-queue", e =>
                    {
                        e.ConfigureConsumer<DrByIdConsumer>(ctx);  //bind to queue for DrByid consumer
                    });
                    cfg.ReceiveEndpoint("dr-proupdate-queue", e =>
                    {
                        e.ConfigureConsumer<DrProfileUpdationConsumer>(ctx);  //bind to queue for DrUpdation consumer
                    });
                    cfg.ReceiveEndpoint("get-all-drs", e =>
                    {
                        e.ConfigureConsumer<GetAllDrsConsumer>(ctx);  
                    });

                    cfg.ReceiveEndpoint("patient-byid-queue", e =>
                    {
                        e.ConfigureConsumer<PatientByIdConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("get-all-patients", e =>
                    {
                        e.ConfigureConsumer<GetAllPatientConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint("patient-block-unblock", e =>
                    {
                        e.ConfigureConsumer<PatientBlockUnblockConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("dr-block-unblock", e =>
                    {
                        e.ConfigureConsumer<DrBlockUnblockConsumer>(ctx);
                    });

                });
                config.AddRequestClient<SpecializationExistsReq>(new Uri("queue:spl-exists-queue"));   //spelization esits request
                config.AddRequestClient<GetAllSpecializationReq>(new Uri("queue:spl-getall-queue"));    //spl get all request
            });
        }
    }
}
