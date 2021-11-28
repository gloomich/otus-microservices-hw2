using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserApi.DataAccess
{
    public static class DbExtensions
    {
        public static string ParseConnectionString(this IConfiguration configuration)
        {
            //var cs = $"Host=localhost;Database=kt_users;Username=user;Password=qwerty;";
            return $"Host={configuration["POSTGRES_HOST"]};" +
                $"Port={configuration["POSTGRES_PORT"]};" +
                $"Database={configuration["POSTGRES_DB"]};" +
                $"Username={configuration["POSTGRES_USER"]};" +
                $"Password={configuration["POSTGRES_PASSWORD"]}";
        }

        public static void RunMigrations(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UserDbContext>(opt =>
                opt.UseNpgsql(builder.Configuration.ParseConnectionString()));

            var app = builder.Build();

            using IServiceScope serviceScope = app.Services
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            var context = serviceScope.ServiceProvider.GetService<UserDbContext>();
            context?.Database.Migrate();
        }

        public static IServiceCollection UseUserDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks().AddDbContextCheck<UserDbContext>();
            services.AddDbContext<UserDbContext>(opt =>
                opt.UseNpgsql(configuration.ParseConnectionString()));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
