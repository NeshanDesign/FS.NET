using System.Web;
using FsNet.Core.Bootstrapping;
using Microsoft.AspNet.Identity.Owin;

namespace FsNet.Core.Account
{
    public class UserHandler
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserHandler()
        {
        }

        public UserHandler(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
    }
}
