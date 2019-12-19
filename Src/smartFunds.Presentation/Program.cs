using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace smartFunds.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Limits.MinResponseDataRate = null;
                })
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}
