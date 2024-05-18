using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventBookSystem.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = configuration["JwtKey"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new ArgumentNullException("JwtKey", "JWT key cannot be null or empty.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = configuration["JwtSettings:ValidIssuer"],
                    ValidAudience = configuration["JwtSettings:ValidAudience"]
                };
            });
        }
    }
}