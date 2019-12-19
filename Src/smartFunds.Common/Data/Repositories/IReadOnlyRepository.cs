using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace smartFunds.Common.Data.Repositories
{
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllAsync(string include = "");            
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, string include = "");        
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, string include = "");
        Task BuildCacheAsync();
        IQueryable<T> BuildIncludeQuery(IQueryable<T> query, string include);
    }
}
