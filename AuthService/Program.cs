
using AuthService.Application.Commands.Patient_authCmd;
using AuthService.Infrastructure.Configurations_;
using DotNetEnv;


namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //services
            builder.Services.ServiceConfiguration();


            //MediatoR
            builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(PatientRegisterCommand).Assembly));

            //repose
            builder.Services.RepoConfiguration();


            //connection
            Env.Load();
            builder.Services.AddDatabaseConfiguration();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
