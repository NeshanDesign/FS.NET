using FsNet.Core.Service.Bootstrapping;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FsNet.UI.User.Startup))]
namespace FsNet.UI.User
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            StartupConfigurator.ConfigureOwin(app);
        }
    }
}
