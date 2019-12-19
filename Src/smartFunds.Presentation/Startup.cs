using AutoMapper;
using Hangfire;
using smartFunds.Presentation.Middleware;
using smartFunds.Presentation.Start;
using smartFunds.Common;
using smartFunds.Data.Initializer;
using smartFunds.Service.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using System.Reflection;
using smartFunds.Core;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(ServiceProfile)));
            services.AllowAllCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            IocConfig.Register(services, Configuration);
            AppSettingsConfig.Register(services, Configuration);
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString(Constants.Database.smartFundsConnectionStringName)));           

            services.UseJwtAuthentication(new DigitalAppsAudience(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(CorsPolicy.AllowAll);
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseErrorHandlingMiddleware();
            app.UseErrorLoggingMiddleware();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

#if DEBUG
            app.ApplicationServices.GetService<smartFundsDatabaseInitializerToMigrate>().Init();
#endif

            app.UseHangfireServer();
            app.UseHangfireDashboard();

        }
    }
}
