
using smartFunds.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace smartFunds.Core
{
    public static class AppSettingsConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<smartFundsRedisOptions>(configuration.GetSection("smartFundsRedis"));
            services.Configure<AppSettingsOptions>(configuration.GetSection("AppSettings"));
            services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));
        }
    }
}
