/*
 * Requied: for migrations
 * ref: https://stackoverflow.com/questions/45782446/unable-to-create-migrations-after-upgrading-to-asp-net-core-2-0
 * For cli:
 * dotnet ef migrations add InitialMigration -s ../smartFunds.Presentation/
 * dotnet ef database update -s ../smartFunds.Presentation/
 */
using smartFunds.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace smartFunds.Presentation.Utils
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<smartFundsDbContext>
    {
        public smartFundsDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<smartFundsDbContext>();
            var connectionString = configuration.GetConnectionString("smartFundsDatabase");
            builder.UseSqlServer(connectionString);
            return new smartFundsDbContext(builder.Options);
        }
    }
}
