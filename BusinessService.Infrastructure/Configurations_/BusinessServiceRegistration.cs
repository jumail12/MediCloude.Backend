using BusinessService.Aplication.Interfaces.IRepos;
using BusinessService.Aplication.Interfaces.IServices;
using BusinessService.Infrastructure.Consumers.Specialization;
using BusinessService.Infrastructure.Persistance;
using BusinessService.Infrastructure.Repositories;
using BusinessService.Infrastructure.Services;
using Contarcts.Requests.Doctor;
using Contarcts.Requests.Patient;
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
                services.AddScoped<IDrAvailabilityRepo,DrAvailabilityRepo>();
                services.AddScoped<IPaymentRepo,PaymentRepo>();
                services.AddScoped<IAppoinmentRepo,AppoinmentRepo>();
            services.AddScoped<IPaymentAppoinmentRepo, PaymentAppoinmentRepo>();
            services.AddScoped<IPrescriptionRepo,PrescriptionRepo>();
        }

        public static void ServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            });
            services.AddScoped<IGeminiService, GeminiService>();

            var _apiKey = Environment.GetEnvironmentVariable("Gemini_APiKey");

            services.AddHttpClient("GeminiClient", client =>
            {
                client.BaseAddress = new Uri($"https://generativelanguage.googleapis.com/");
            });

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

                //request config with queue
                config.AddRequestClient<DrByIdReq>(new Uri("queue:dr-queue"));   //get dr by id
                config.AddRequestClient<DrProfileUpdationReq>(new Uri("queue:dr-proupdate-queue"));
                config.AddRequestClient<GetAllDrsReq>(new Uri("queue:get-all-drs"));
                config.AddRequestClient<PatientByIdRequest>(new Uri("queue:patient-byid-queue"));
                config.AddRequestClient<GetAllPatientReq>(new Uri("queue:get-all-patients"));
                config.AddRequestClient<PatientBlockUnblockReq>(new Uri("queue:patient-block-unblock"));
                config.AddRequestClient<DrBlockUnBlockReq>(new Uri("queue:dr-block-unblock"));
            });
        }
    }
}
