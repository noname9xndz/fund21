using Microsoft.Extensions.Configuration;

namespace smartFunds.Presentation.Middleware
{
    public class DigitalAppsAudience
    {
        public readonly IConfiguration _configuration;
        public DigitalAppsAudience(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Name = "All digital resourses";
        public string AudienceId = "AFD506D888A54947B2AD24B2A2D3041A";
        public string Base64Secret = "IyrAkDob2KqGlO6IhrSrUJELhUckeQEPVpaePlS_Daq";
        public string TokenIssuer
        {
            get
            {
                return _configuration.GetValue<string>("AppSettings:TokenIssuer");
            }
        }

    }
}
