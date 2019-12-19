using Microsoft.Extensions.DependencyInjection;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Start
{
    public static class CorsExtension
    {
        public static IServiceCollection AllowAllCors(this IServiceCollection services)
        {
            return services.AddCors(o => o.AddPolicy(CorsPolicy.AllowAll, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));           
        }
    }
}
