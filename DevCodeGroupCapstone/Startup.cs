using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevCodeGroupCapstone.Startup))]
namespace DevCodeGroupCapstone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
