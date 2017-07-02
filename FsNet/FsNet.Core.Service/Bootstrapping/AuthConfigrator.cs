using System;
using FsNet.Data.Domain.User;
using FsNet.Data.EF.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace FsNet.Core.Service.Bootstrapping
{
    public class AuthConfigrator
    {
        public static void Run(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app = app.CreatePerOwinContext(DataContext.Create);
            app = app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //   app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app = app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            CookieConfigurator(app);
            ExternalLoginsConfigurator(app);
        }


        private static void CookieConfigurator(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<
                        ApplicationUserManager,
                        ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
        private static void ExternalLoginsConfigurator(IAppBuilder app)
        {
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}