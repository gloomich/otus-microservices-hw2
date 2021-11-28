using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserApi.DataAccess
{
    public class DesignTimeDbContextFactory :
        IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddUserSecrets<DesignTimeDbContextFactory>()
#endif
                .Build();
            var builder = new DbContextOptionsBuilder<UserDbContext>();            
            builder.UseNpgsql(configuration.ParseConnectionString());
            return new UserDbContext(builder.Options);
        }
    }
}
