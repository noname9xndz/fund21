using smartFunds.Common.Data;
using smartFunds.Common.Options;
using smartFunds.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Initializer
{
    public class smartFundsDatabaseInitializerToMigrate
    {
        private readonly IDbContextFactory<smartFundsDbContext> _smartFundsDbContextFactory;
        private readonly IOptions<ConnectionStringsOptions> _connectionStringsOptions;
        public smartFundsDatabaseInitializerToMigrate(IDbContextFactory<smartFundsDbContext> smartFundsDbContextFactory, IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            _smartFundsDbContextFactory = smartFundsDbContextFactory;
            _connectionStringsOptions = connectionStringsOptions;
        }

        public void Init()
        {
            using (var context = _smartFundsDbContextFactory.GetContext())
            {

                if (!context.AllMigrationsApplied())
                {
                    context.Database.Migrate();
                    
                }
                context.EnsureSeeded();                
            }
        }
    }
}
