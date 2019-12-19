using smartFunds.Caching.AutoComplete;
using smartFunds.Common;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace smartFunds.Data.Repositories.Generic
{
    public abstract class GenericPersistentTrackedRepository<T> : GenericRepository<T> where T : class, ITrackedEntity, IPersistentEntity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected GenericPersistentTrackedRepository(smartFundsDbContext dbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(dbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected string CurrentUser =>
            _httpContextAccessor != null && _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.Identity.Name
                : "Unknown";

        public override T Add(T entity)
        {
            entity.DateLastUpdated = DateTime.Now;
            entity.LastUpdatedBy = CurrentUser;
            entity.DeletedAt = "NA";
            return base.Add(entity);
        }

        public override void Update(T entity)
        {
            entity.DateLastUpdated = DateTime.Now;
            entity.LastUpdatedBy = CurrentUser;
            base.Update(entity);
        }

        public override void BulkUpdate(ICollection<T> items)
        {
            if (items == null || !items.Any())
                return;

            base.BulkUpdate(items.Select(x => {x.DateLastUpdated = DateTime.Now; x.LastUpdatedBy = CurrentUser; return x; }).ToList());
        }


        public override void BulkInsert(ICollection<T> items)
        {
            base.BulkInsert(items.Select(x => { x.DateLastUpdated = DateTime.Now; x.LastUpdatedBy = CurrentUser; return x; }).ToArray());
        }

        public override void Delete(T entity)
        {
            if (entity == null)
                return;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now.ToString(Constants.DateTimeFormats.IsoLongDateTime);
            entity.LastUpdatedBy = CurrentUser;
            entity.DateLastUpdated = DateTime.Now;

            Update(entity);
        }

        public override void BulkDelete(ICollection<T> items)
        {
            if (items == null || !items.Any())
                return;

            BulkUpdate(items.Select(x =>
            {
                x.IsDeleted = true; x.DeletedAt = DateTime.Now.ToString(Constants.DateTimeFormats.IsoLongDateTime); return x; 
            }).ToList());
        }

    }
}
