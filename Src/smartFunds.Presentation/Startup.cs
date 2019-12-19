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
using Microsoft.AspNetCore.Identity;
using System;
using smartFunds.Data.Models;
using smartFunds.Data;
using System.Collections.Generic;
using smartFunds.Business;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<smartFundsDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(1);
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Admin/Login";
                options.AccessDeniedPath = "/error/access-denied";
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAccountantAccess", policy => policy.RequireRole(RoleName.Accountant));
                options.AddPolicy("OnlyAdminAccess", policy => policy.RequireRole(RoleName.Admin));
                options.AddPolicy("AdminManagerAccess", policy => policy.RequireRole(RoleName.Admin, RoleName.CustomerManager, RoleName.InvestmentManager, RoleName.Accountant));
                options.AddPolicy("CustomerManagerNotAccess", policy => policy.RequireRole(RoleName.Admin, RoleName.InvestmentManager, RoleName.Accountant));
                options.AddPolicy("CustomerAccess", policy => policy.RequireRole(RoleName.Customer));
                options.AddPolicy("AccountantInvestmentManagerAccess", policy => policy.RequireRole(RoleName.InvestmentManager, RoleName.Accountant));
                options.AddPolicy("AdminInvestmentManagerAccess", policy => policy.RequireRole(RoleName.Admin, RoleName.InvestmentManager));
                options.AddPolicy("AdminAccountantAccess", policy => policy.RequireRole(RoleName.Admin, RoleName.Accountant));
            });

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
            

            app.UseCors(CorsPolicy.AllowAll);
            app.UseAuthentication();
            app.UseHttpsRedirection();

            // Return static files and end the pipeline.
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for 30 days
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
                }
            });

            app.UseErrorHandlingMiddleware();
            app.UseErrorLoggingMiddleware();

            //app.UseDeveloperExceptionPage();
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/404";
                    await next();
                }
            });
            //app.UseStatusCodePagesWithReExecute("/error/500");
            app.UseExceptionHandler("/error/500");

            app.UseMvcWithDefaultRoute();

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

            HangfireJobScheduler.ScheduleRecurringJobs();
            
        }
    }
}
