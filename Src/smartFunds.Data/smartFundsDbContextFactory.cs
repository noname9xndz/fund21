using smartFunds.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace smartFunds.Data
{

    public class smartFundsDbContextFactory : IDbContextFactory<smartFundsDbContext>
    {
        private readonly DbContextOptions<smartFundsDbContext> _options;
        public smartFundsDbContextFactory(DbContextOptions<smartFundsDbContext> options) {
            _options = options;
        }
        public smartFundsDbContext GetContext()
        {
            var context = new smartFundsDbContext(_options);
            return context;
        }
    }
}
