using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UltimaNeo.Web.Client.Startup))]
namespace UltimaNeo.Web.Client
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
