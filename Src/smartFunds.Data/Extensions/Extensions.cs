using System;
using System.Linq;
using smartFunds.Common;
using smartFunds.Common.Data.Repositories;
using smartFunds.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Extensions
{
    public static class Extensions
    {
        public static void SoftDelete<T>(this DbContext ctx, T item, string currentUser) where T : class, IPersistentEntity, ITrackedEntity
        {
            if (item == null) return;

            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now.ToString(Constants.DateTimeFormats.IsoLongDateTime);
            item.DateLastUpdated = DateTime.Now;
            item.LastUpdatedBy = currentUser;
            ctx.Update(item);
        }

        public static void TrackedEntity<T>(this DbContext ctx, T item, string currentUser) where T : class, ITrackedEntity
        {
            if (item == null) return;

            item.DateLastUpdated = DateTime.Now;
            item.LastUpdatedBy = currentUser;
            ctx.Update(item);
        }

        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this smartFundsDbContext context)
        {
            SeedDataHelper.Seed(context);
            context.SaveChanges();
        }
       
    }
}
