using smartFunds.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace smartFunds.Presentation.Start
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
