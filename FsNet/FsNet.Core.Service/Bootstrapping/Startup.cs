using System.Data.Entity;
using FsNet.Data.EF.Context;
using FsNet.Data.EF.Migrations;
using Ninject;
using Owin;

namespace FsNet.Core.Service.Bootstrapping
{
    public class StartupConfigurator
    {
        public static void ConfigureOwin(IAppBuilder app)
        {
            AuthConfigrator.Run(app);


            

        }

        public static void ConfigureNinject(IKernel kernel)
        {
            DiConfigurator.Run(kernel);
        }
    }
}
