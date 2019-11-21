using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_3.Startup))]
namespace Project_3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
