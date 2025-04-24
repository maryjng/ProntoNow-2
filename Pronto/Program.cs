using Pronto.Data;
using Pronto.Repositories;
using Pronto.Repositories.Interfaces;
using Pronto.Services;

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
                webBuilder.ConfigureServices(services =>
                {
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
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
}
