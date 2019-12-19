using smartFunds.Common.Data;
using smartFunds.Common.Options;
using smartFunds.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Initializer
{
    public class smartFundsDatabaseInitializerToRecreate
    {
        private readonly IDbContextFactory<smartFundsDbContext> _smartFundsDbContextFactory;
        private readonly IOptions<ConnectionStringsOptions> _connectionStringsOptions;
        public smartFundsDatabaseInitializerToRecreate(IDbContextFactory<smartFundsDbContext> smartFundsDbContextFactory, IOptions<ConnectionStringsOptions> connectionStringsOption)
        {
            _smartFundsDbContextFactory = smartFundsDbContextFactory;
            _connectionStringsOptions = connectionStringsOption;
        }

        public void Init()
        {
            using (var context = _smartFundsDbContextFactory.GetContext())
            {
                if (context.Database.EnsureDeleted())
                {
                    context.Database.Migrate();
                    context.EnsureSeeded();                    
                }
            }
        }
    }
}
