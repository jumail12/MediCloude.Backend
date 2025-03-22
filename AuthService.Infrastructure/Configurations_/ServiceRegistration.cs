using AuthService.Application.Interfaces.IRepos;
using AuthService.Application.Interfaces.IServices;
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
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(host, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                });
                config.AddRequestClient<SpecializationExistsReq>(new Uri("queue:spl-exists-queue"));   //spelization esits request
                config.AddRequestClient<GetAllSpecializationReq>(new Uri("queue:spl-getall-queue"));    //spl get all request
            });
        }
    }
}
