using System;
using smartFunds.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using smartFunds.Common.Data.Repositories;
using smartFunds.Data.Models.Contactbase;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace smartFunds.Data
{
    public class smartFundsDbContext : IdentityDbContext<User>
    {
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        public smartFundsDbContext(DbContextOptions<smartFundsDbContext> options) : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }
        
        public DbSet<Models.HangFire.Job> Jobs { get; set; }
        public DbSet<Interchange> Interchanges { get; set; }
        public DbSet<InterchangeLocality> InterchangeLocalities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSublocality> EventSublocalities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<Sublocality> Sublocalities { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Host> Hosts { get; set; }
        public DbSet<EventHost> EventHosts { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }
        public DbSet<MealAllocation> MealAllocations { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<KVRRQuestion> KvrrQuestion { get; set; }
        public DbSet<KVRRAnswer> KvrrAnswer { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Config index
            modelBuilder.Entity<Interchange>().HasIndex(x => new { x.MainLocalityId, x.IsDeleted, x.DeletedAt }).IsUnique();
            modelBuilder.Entity<Host>().HasIndex(x => x.HouseholderId);
            modelBuilder.Entity<EventHost>().HasIndex(x => new { x.EventId, x.HostId });
            modelBuilder.Entity<EventGuest>().HasIndex(x => new { x.EventId, x.MemberId, x.HouseholderId });

            // Disable cascade delete system wide
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Filter out all deleted data
            FilterDeletedRecords(modelBuilder);

            modelBuilder.Entity<Region>().ToTable("RegionView", "dbo");
            modelBuilder.Entity<Country>().ToTable("CountryView", "dbo");
            modelBuilder.Entity<Sublocality>().ToTable("SublocalityView", "dbo");
            modelBuilder.Entity<Locality>().ToTable("LocalityView", "dbo");
            modelBuilder.Entity<Member>().ToTable("MemberView", "dbo");
            

            base.OnModelCreating(modelBuilder);
        }

        #region Private Method

        private void FilterDeletedRecords(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IPersistentEntity).IsAssignableFrom(entity.ClrType))
                {
                    modelBuilder
                        .Entity(entity.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var param = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, param, Expression.Constant("IsDeleted"));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, param);
            return lambda;
        }

        #endregion


    }
}
