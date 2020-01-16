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
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly smartFundsDbContext _context;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private readonly IRedisAutoCompleteProvider _redisAutoCompleteProvider;
        public GenericRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider)
        {
            _context = smartFundsDbContext;
            _redisConfigurationOptions = redisConfigurationOptions;
            _redisAutoCompleteProvider = redisAutoCompleteProvider;
        }

        public virtual T Add(T entity)
        {
            var result = _context.Set<T>().Add(entity);
            return result.Entity;

        }

        public virtual void BulkInsert(ICollection<T> items)
        {
            _context.Set<T>().AddRange(items);
        }

        public virtual void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void BulkUpdate(ICollection<T> items)
        {
            foreach (var entity in items)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                return;

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity);
            }
        }

        public virtual void BulkDelete(ICollection<T> items)
        {
            if (items == null || !items.Any())
                return;
            _context.Set<T>().RemoveRange(items);                      
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, string include = "")
        {
            var query = _context.Set<T>().AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();
            return await Task.FromResult(query.Where(predicate).FirstOrDefault());
        }

        public virtual async Task<IQueryable<T>> GetAllAsync(string include = "")
        {
            var query = _context.Set<T>().AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();
            return query;
        }

        public virtual async Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> predicate, string include = "")
        {
            var query = _context.Set<T>().Where(predicate).AsQueryable<T>().AsNoTracking();
            query = BuildIncludeQuery(query, include).AsExpandable();

            return query;

        }

        public virtual async Task BuildCacheAsync()
        {
            if (_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                var interfaces = typeof(T).GetInterfaces().Select(x => x.Name).ToList();

                if (interfaces.Any(x => x.ToLower().Contains("iautocomplete")))
                {
                    var allItems = (await GetAllAsync()).ToList();

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

        public async Task ExecuteSql(string statement)
        {
            await _context.Database.ExecuteSqlCommandAsync(statement);
        }

       
    }
}



