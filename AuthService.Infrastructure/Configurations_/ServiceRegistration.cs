
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Persistance;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
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
        }

        public static void ServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }



    }
}
