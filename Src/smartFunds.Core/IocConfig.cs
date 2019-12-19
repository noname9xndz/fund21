using smartFunds.Caching;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common;
using smartFunds.Common.Data;
using smartFunds.Data;
using smartFunds.Data.Initializer;
using smartFunds.Data.Repositories;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using smartFunds.Business;
using smartFunds.Infrastructure;

namespace smartFunds.Core
{
  public static class IocConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            // Assembly scanning
            services.Scan(scan => scan
                .FromAssemblyOf<ISettingRepository>().AddClasses().AsSelfWithInterfaces().WithTransientLifetime()
                .FromAssemblyOf<IRedisCacheProvider>().AddClasses().AsSelfWithInterfaces().WithTransientLifetime()
                .FromAssemblyOf<ISettingService>().AddClasses().AsSelfWithInterfaces().WithTransientLifetime()
                .FromAssemblyOf<ITestManager>().AddClasses().AsSelfWithInterfaces().WithTransientLifetime()
                .FromAssemblyOf<ISMSGateway>().AddClasses().AsSelfWithInterfaces().WithTransientLifetime());

            // Context
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Db
            services.AddSingleton(new DbContextOptionsBuilder<smartFundsDbContext>().UseSqlServer(configuration.GetConnectionString(Constants.Database.smartFundsConnectionStringName)).Options);
            services.AddTransient<smartFundsDatabaseInitializerToMigrate, smartFundsDatabaseInitializerToMigrate>();
            services.AddTransient<IDbContextFactory<smartFundsDbContext>, smartFundsDbContextFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Cache
            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            services.AddTransient<IRedisCacheProvider, RedisCacheProvider>();
            services.AddTransient<IRedisAutoComplete, RedisAutoComplete>();
            services.AddTransient<IRedisAutoCompleteProvider, RedisAutoCompleteProvider>();
        }
    }
}
