
using BusinessService.Aplication.Commands.SpecializationDrCommand;
using BusinessService.Infrastructure.Configurations_;
using BusinessService.Middlewares;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BusinessService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

       

            builder.Services.AddControllers();
        
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MEDICLOUDE BUSINESS API", Version = "v1" });


                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //database con
            Env.Load();
            builder.Services.AddDatabaseConfiguration();

            //service
            builder.Services.ServiceConfiguration();

            //repos
            builder.Services.RepoConfiguration();

            //MediatoR
            builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(SpecializationDrAddCmd).Assembly));

            //rabbitMQ
            builder.Services.RabbitMqConfig();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(o =>
           {
               var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

               if (string.IsNullOrEmpty(jwtKey))
               {
                   throw new Exception("JWT key is missing from environment variables.");
               }

               o.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ClockSkew = TimeSpan.Zero
               };
           });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                    });
            });


            var app = builder.Build();
          
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<BusinessServiceUserIdMiddlware>();
            app.MapControllers();

            app.Run();
        }
    }
}
