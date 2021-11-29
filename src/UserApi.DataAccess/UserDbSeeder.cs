using Microsoft.Extensions.DependencyInjection;

namespace UserApi.DataAccess
{
    public static class UserDbSeeder
    {
        internal static void Seed(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetService<UserDbContext>();
            if (dbContext != null && dbContext.Database.EnsureCreated())
            {
                if (!dbContext.Users.Any())
                {
                    dbContext.Users.AddRange(new List<User>
                        {
                            new User{ FirstName = "Ivan", LastName = "Ivanov" },
                            new User{ FirstName = "Petr", LastName = "Petrov" },
                            new User{ FirstName = "Sidor", LastName = "Sidorov" }
                        });
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
