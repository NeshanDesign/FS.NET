using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Activation;
using FsNet.Core.Service.AccountService;
using FsNet.Data.Domain.User;
using Microsoft.Owin.Security;

namespace FsNet.Core.Service.Bootstrapping
{
    public class DiConfigurator
    {

        public static void Run(IKernel kernel)
        {
            //kernel.Bind<ApplicationSignInManager>().ToProvider<SinInManagerProvider>();
            //kernel.Bind<ApplicationUserManager>().ToProvider<UserManagerProvider>();
            //kernel.Bind<IAuthenticationManager>().ToProvider<AuthenticationManagerProvider>();
            //kernel.Bind<UserManager<ApplicationUser>>().ToSelf();
            //kernel.Bind<IUserManagementService>().To<UserManagementService>();
        }

    }
}