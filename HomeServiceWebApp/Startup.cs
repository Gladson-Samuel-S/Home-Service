using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeServiceWebApp.Startup))]
namespace HomeServiceWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
