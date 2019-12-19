using smartFunds.Common.Exceptions;

namespace smartFunds.Core
{
    public class ExeHostingEnvironment
    {
        private readonly HostingEnvironmentType _hostingEnvironmentType;
        public ExeHostingEnvironment()
        {
            _hostingEnvironmentType = HostingEnvironmentType.Development;
        }
        public ExeHostingEnvironment(string environment)
        {
            switch (environment)
            {
                case "Development":
                    _hostingEnvironmentType = HostingEnvironmentType.Development;
                    break;
                case "Smartosc":
                    _hostingEnvironmentType = HostingEnvironmentType.Smartosc;
                    break;
                case "Staging":
                    _hostingEnvironmentType = HostingEnvironmentType.Staging;
                    break;
                case "Uat":
                    _hostingEnvironmentType = HostingEnvironmentType.Uat;
                    break;
                case "Production":
                    _hostingEnvironmentType = HostingEnvironmentType.Production;
                    break;
                default:
                    throw new InvalidParameterException();
            }
        }

        public override string ToString()
        {
            return _hostingEnvironmentType.ToString();
        }
    }

    enum HostingEnvironmentType {
        Development,
        Smartosc,
        Staging,
        Uat,
        Production
    }
}
