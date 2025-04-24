using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pronto.Data;
using Pronto.Repositories;
using Pronto.Repositories.Interfaces;
using Pronto.Services;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices((context, services) =>
                {
                    var jwtSettings = context.Configuration.GetSection("Jwt");
                    var secretKey = jwtSettings["Key"];

                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings["Issuer"],
                            ValidAudience = jwtSettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                        };
                    });

                    services.AddAuthorization();

                    services.AddControllers();
                    services.AddScoped<UserRepository>();
                    services.AddScoped<DeviceRepository>();
                    services.AddScoped<BusinessRepository>();
                    services.AddScoped<ReportRepository>();
                    services.AddScoped<DatabaseHelper>();
                    services.AddScoped<IDatabaseHelper, DatabaseHelper>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IPasswordHasherService, PasswordHasherService>();
                    services.AddScoped<IJwtTokenService, JwtTokenService>();


                    services.AddControllers();
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
}
