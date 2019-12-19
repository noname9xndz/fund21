using smartFunds.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace smartFunds.Presentation.Start
{
    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }

    }
}
