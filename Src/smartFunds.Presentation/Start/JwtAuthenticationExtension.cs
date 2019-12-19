using smartFunds.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace smartFunds.Presentation.Start
{
    public static class JwtAuthenticationExtension
    {
        public static Microsoft.AspNetCore.Authentication.AuthenticationBuilder UseJwtAuthentication(this IServiceCollection builder, DigitalAppsAudience digitalApp)
        {
            return builder.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = digitalApp.TokenIssuer,
                    ValidAudience = digitalApp.AudienceId,
                    IssuerSigningKey = new SymmetricSecurityKey(WebEncoders.Base64UrlDecode(digitalApp.Base64Secret)),
                    ClockSkew = System.TimeSpan.Zero
                };
            });
        }
    }
}
