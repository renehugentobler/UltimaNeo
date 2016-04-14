using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UltimaNeo.Web.CC.Startup))]
namespace UltimaNeo.Web.CC
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
