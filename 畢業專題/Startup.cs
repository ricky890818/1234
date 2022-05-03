using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(畢業專題.Startup))]
namespace 畢業專題
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
