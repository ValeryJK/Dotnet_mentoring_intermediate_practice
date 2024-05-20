using EventBookSystem.API.ActionFilters;
using EventBookSystem.API.Extensions;
using EventBookSystem.Core.Service;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace EventBookSystem.API
{
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            builder.Services.AddAuthentication();
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.ConfigureJWT(builder.Configuration);

            builder.Services.AddScoped<ValidationFilterAttribute>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Event Book System API",
                    Version = "v1"
                });
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Event Book System API",
                    Version = "v2"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                s.IncludeXmlComments(xmlPath);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                     new OpenApiSecurityScheme
                     {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                     },
                     new List<string>()
                  }
                });
            });

            builder.Logging.AddConsole();

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            app.ConfigureExceptionHandler(logger);

            if (app.Environment.IsProduction())
                app.UseHsts();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Book System API v1");
                    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Event Book System API v2");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
