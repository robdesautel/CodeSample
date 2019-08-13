using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owin;
//using PoolReservation.Infrastructure.Hangfire.Authentication;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(PoolReservation.App_Start.Startup))]
namespace PoolReservation.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            });

            ConfigureAuth(app);

            var options = new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(15) // Default value
            };

            var daKey = ConfigurationManager.AppSettings["StripeApiKey"];

            StripeConfiguration.SetApiKey(daKey);

            Hangfire.GlobalConfiguration.Configuration
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["PoolReservation"].ConnectionString, options);

            app.UseHangfireServer();

            app.UseHangfireDashboard("/hangfire");

            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireIdentityAuthentication() }
            //});
        }
    }
}