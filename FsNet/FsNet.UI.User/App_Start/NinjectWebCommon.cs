using FsNet.Core.Service.Bootstrapping;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FsNet.UI.User.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FsNet.UI.User.App_Start.NinjectWebCommon), "Stop")]

namespace FsNet.UI.User.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Ninject.Activation;
    using FsNet.Core.Service.AccountService;
    using FsNet.Data.Domain.User;
    using Microsoft.Owin.Security;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAuthenticationManager>().ToProvider<AuthenticationManagerProvider>();
            kernel.Bind<ApplicationUserManager>().ToProvider<UserManagerProvider>();
            kernel.Bind<ApplicationSignInManager>().ToProvider<SinInManagerProvider>();
            kernel.Bind<IUserManagementService>().To<UserManagementService>();
        }

        internal class SinInManagerProvider : Provider<ApplicationSignInManager>
        {
            protected override ApplicationSignInManager CreateInstance(IContext context)
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        internal class UserManagerProvider : Provider<ApplicationUserManager>
        {
            protected override ApplicationUserManager CreateInstance(IContext context)
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
            }
        }

        internal class AuthenticationManagerProvider : Provider<IAuthenticationManager>
        {
            protected override IAuthenticationManager CreateInstance(IContext context)
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
    }
}
