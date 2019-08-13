using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PoolReservationTemplateMVC.Startup))]
namespace PoolReservationTemplateMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
