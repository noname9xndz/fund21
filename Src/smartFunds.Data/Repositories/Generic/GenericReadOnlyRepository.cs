using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace smartFunds.Data.Repositories.Generic
{
    public abstract class ReadOnlyGenericRepository<T> : IReadOnlyRepository<T> where T : class
    {
        private readonly smartFundsDbContext _context;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private readonly IRedisAutoCompleteProvider _redisAutoCompleteProvider;

        protected ReadOnlyGenericRepository(smartFundsDbContext dbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
           , IRedisAutoCompleteProvider redisAutoCompleteProvider)
        {
            _context = dbContext;
            _redisConfigurationOptions = redisConfigurationOptions;
            _redisAutoCompleteProvider = redisAutoCompleteProvider;
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, string include = "")
        {
            var query = _context.Set<T>().AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();
            return await Task.FromResult(query.Where(predicate).FirstOrDefault());
        }

        public virtual async Task<ICollection<T>> GetAllAsync(string include = "")
        {
            var query = _context.Set<T>().AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();
            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, string include = "")
        {
            var query = _context.Set<T>().Where(predicate).AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();
            return await query.ToListAsync();
        }

        public async Task BuildCacheAsync()
        {
            if (_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                var interfaces = typeof(T).GetInterfaces().Select(x => x.Name).ToList();

                if (interfaces.Any(x => x.ToLower().Contains("iautocomplete")))
                {
                    var allItems = await GetAllAsync();

                    if (allItems != null && allItems.Count > 0)
                    {
                        var autoItems = allItems.Select(x => x as IAutoCompleteModel);
                        await _redisAutoCompleteProvider.ClearAsync(autoItems.First().AutoCompleteItem.SetName);
                        await _redisAutoCompleteProvider.AddAsync(autoItems.ToList());
                    }
                }
            }
        }

        public IQueryable<T> BuildIncludeQuery(IQueryable<T> query, string include)
        {
            if (string.IsNullOrWhiteSpace(include))
                return query;

            var includeEntities = include.Split(',');
            if (includeEntities == null || !includeEntities.Any())
                return query;

            foreach (var entity in includeEntities)
            {
                query = query.Include(entity);
            }

            return query;
        }
    }
}
