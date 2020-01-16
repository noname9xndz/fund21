using System;
using smartFunds.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using smartFunds.Common.Data.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace smartFunds.Data
{
    public class smartFundsDbContext : IdentityDbContext<User>
    {
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        public smartFundsDbContext(DbContextOptions<smartFundsDbContext> options) : base(options)
        {
        }
        public DbSet<Models.HangFire.Job> Jobs { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<FundPurchaseFee> FundPurchaseFees { get; set; }
        public DbSet<FundSellFee> FundSellFees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRequest> OrderRequests { get; set; }
        public DbSet<KVRRQuestion> KvrrQuestion { get; set; }
        public DbSet<KVRRAnswer> KvrrAnswer { get; set; }
        public DbSet<KVRR> Kvrr { get; set; }
        public DbSet<KVRRMark> KvrrMark { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<KVRRPortfolio> KVRRPortfolio { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<InvestmentTarget> InvestmentTargets { get; set; }
        public DbSet<Fund> Fund { get; set; }
        public DbSet<PortfolioFund> PortfolioFund { get; set; }
        public DbSet<AdminTask> Tasks { get; set; }
        public DbSet<UserFund> UserFunds { get; set; }
        public DbSet<FundTransactionHistory> FundTransactionHistory { get; set; }
        public DbSet<ContactCMS> ContactConfigurations { get; set;}
        public DbSet<HomepageCMS> HomepageConfigurations { get; set;}
        public DbSet<GenericIntroducingSetting> GenericIntroducingSettings { get; set; }
        public DbSet<InvestmentTargetSetting> InvestmentTargetSettings { get; set; }
        public DbSet<CustomerLevel> CustomerLevel { get; set; }
        public DbSet<MaintainingFee> MaintainingFees { get; set; }
        public DbSet<Investment> Investment { get; set; }
        public DbSet<TaskCompleted> TasksCompleted { get; set; }
        public DbSet<WithdrawalFee> WithdrawalFees { get; set; }
        public DbSet<GlobalConfiguration> GlobalConfiguration { get; set; }

        public DbSet<CateNews> CateNews { get; set; }
        public DbSet<ContentNews> ContentNews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                        

            // Disable cascade delete system wide
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Filter out all deleted data
            FilterDeletedRecords(modelBuilder);
            

            modelBuilder.Entity<KVRR>().HasKey(x => x.Id);
            modelBuilder.Entity<Portfolio>().HasKey(x => x.Id);
            modelBuilder.Entity<KVRRPortfolio>().HasKey(x => new { x.KVRRId, x.PortfolioId });
            modelBuilder.Entity<KVRRPortfolio>()
                .HasOne(x => x.KVRR)
                .WithMany(m => m.KVRRPortfolios)
                .HasForeignKey(x => x.KVRRId);
            modelBuilder.Entity<KVRRPortfolio>()
                .HasOne(x => x.Portfolio)
                .WithMany(e => e.KVRRPortfolios)
                .HasForeignKey(x => x.PortfolioId);

            modelBuilder.Entity<PortfolioFund>().HasKey(x => new { x.PortfolioId, x.FundId });
            modelBuilder.Entity<PortfolioFund>()
                .HasOne(x => x.Fund)
                .WithMany(m => m.PortfolioFunds)
                .HasForeignKey(x => x.FundId);
            modelBuilder.Entity<PortfolioFund>()
                .HasOne(x => x.Portfolio)
                .WithMany(e => e.PortfolioFunds)
                .HasForeignKey(x => x.PortfolioId);

            modelBuilder.Entity<UserFund>().HasKey(x => new { x.UserId, x.FundId });
            modelBuilder.Entity<UserFund>().HasOne(x => x.Fund).WithMany(m => m.UserFunds).HasForeignKey(x => x.FundId);
            modelBuilder.Entity<UserFund>().HasOne(x => x.User).WithMany(m => m.UserFunds).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<CateNews>().HasKey(x => x.Id);
            modelBuilder.Entity<ContentNews>().HasKey(x => x.Id);

            modelBuilder.Entity<ContentNews>().Property(e => e.Contents).HasColumnType("NTEXT");
            base.OnModelCreating(modelBuilder);
        }

        #region Private Method

        private void FilterDeletedRecords(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IPersistentEntity).IsAssignableFrom(entity.ClrType) && !typeof(User).IsAssignableFrom(entity.ClrType)) //Exclude User type
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
