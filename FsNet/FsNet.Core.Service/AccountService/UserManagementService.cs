using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FsNet.Data.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using FsNet.Core.Service.Bootstrapping;
using FsNet.Data.Domain.User;
using Microsoft.AspNet.Identity.Owin;

namespace FsNet.Core.Service.AccountService
{
    public class UserManagementService : IUserManagementService
    {
        public Guid InstanceId { get; internal set; }
        public  ApplicationSignInManager SignInManager { get; internal set; }
        public ApplicationUserManager UserManager { get; internal set; }
        private readonly IAuthenticationManager _authenticationManager;
        private bool _disposing = true;

        public UserManagementService(ApplicationUserManager userManager, 
            ApplicationSignInManager signInManager, 
            IAuthenticationManager authenticationManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _authenticationManager = authenticationManager;
            InstanceId = Guid.NewGuid();
            
        }

        public async Task<SignInStatus> Login(string username, string password, bool rememberMe = false, bool shouldLockout = false)
        {
                var result = await SignInManager.PasswordSignInAsync(username, password, rememberMe, shouldLockout: shouldLockout);
                return result;
        }

        public async Task<UserViewModel> GetUserInfo(string userId)
        {
            var userInfo = new UserViewModel
            {
                HasPassword = ((Func<bool>)(() =>
                               {
                                   var user = UserManager.FindById(userId);
                                   return user?.PasswordHash != null;
                               }))(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await _authenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return userInfo;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        public ApplicationUser GetUserById(string userId)
        {
            return  UserManager.FindById(userId);
        }

        public async Task<ManageMessageId> RemoveLogin(string userId, string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
            if (!result.Succeeded) return ManageMessageId.Error;
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return ManageMessageId.RemoveLoginSuccess;
        }

        public async Task AddPhoneNumber(string userId, string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
            if (UserManager.SmsService != null)
            {
                //todo: create a method for sms service to send sms with its default template
                // SmsService.Send(stirng msg)
                var message = new IdentityMessage
                {
                    Destination = phoneNumber,
                    Body = "Your security code is: " + code
                };
               await UserManager.SmsService.SendAsync(message);
            }
        }

        public async Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            return await SignInManager.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser);
        }

        public async Task ToggleTwoFactorAuthenticationAsync(string userId, bool enabled)
        {
            await UserManager.SetTwoFactorEnabledAsync(userId, enabled);
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
        }

        public async Task<IdentityResult> SetPassword(string userId, string newPassword)
        {
            var result = await UserManager.AddPasswordAsync(userId, newPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }
            return result;
        }

        public async Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
            return code;
        }

        public async Task<IdentityResult> VerifyPhoneNumber(string userId, string phoneNumber, string token)
        {
            var result = await UserManager.ChangePhoneNumberAsync(userId, phoneNumber, token);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }
            return result;
        }

        public async Task<bool> IsVerlfied()
        {
            return await SignInManager.HasBeenVerifiedAsync();
        }

        public async Task<ManageMessageId> RemovePhoneNumber(string userId)
        {
            var result = await UserManager.SetPhoneNumberAsync(userId, null);
            if (!result.Succeeded)
            {
               return ManageMessageId.Error;
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return ManageMessageId.RemovePhoneSuccess;
        }

        public async Task<ChangePasswordResultModel> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = new ChangePasswordResultModel
            {
                Result = await UserManager.ChangePasswordAsync(userId, oldPassword, newPassword)
            };
            if (result.IsSucceeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                result.MessageId = ManageMessageId.ChangePasswordSuccess;
            }
            return result;
        }

        public async Task<ManageLoginsResult> ManageLogins(string userId)
        {
            var result = new ManageLoginsResult();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ManageLoginsResult(){ GeneralMessage = "No User Found", IsOk = false };
            }
            var userLogins = await UserManager.GetLoginsAsync(userId);
            var otherLogins = _authenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
           result.HasResult = user.PasswordHash != null || userLogins.Count > 1;
            return new ManageLoginsResult
            {
                Logins = userLogins,
                ExternalLogins = otherLogins,
                HasResult = userLogins.Count > 1
            };
        }

        public async Task<ManageMessageId> LinkLoginCallback(string userId, string xsrfKey)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync(xsrfKey, userId);
            if (loginInfo == null)
            {
                return  ManageMessageId.Error;
            }
            var result = await UserManager.AddLoginAsync(userId, loginInfo.Login);
            return result.Succeeded ? ManageMessageId.Null : ManageMessageId.Error ;
        }

        public async Task<string> GetPhoneNumberAsync(string userId)
        {
            return await UserManager.GetPhoneNumberAsync(userId);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(string userId)
        {
            return await UserManager.GetTwoFactorEnabledAsync(userId);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            return await UserManager.GetLoginsAsync(userId);
        }

        public async Task<bool> TwoFactorBrowserRememberedAsync(string userId)
        {
            return await _authenticationManager.TwoFactorBrowserRememberedAsync(userId);
        }


        public async Task<IdentityResult> RegisterAndSignIn(string username, string email, string password)
        {
            var user = new ApplicationUser {UserName = username, Email = email};
            var result = await UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return result;
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            return await UserManager.ConfirmEmailAsync(userId, code);
        }

        public async Task<bool> SendEmailForForgotPassword(string userId, string callbackBaseUrl)
        {
            var user = await UserManager.FindByNameAsync(userId);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(userId)))
                return false;

            var code = await UserManager.GeneratePasswordResetTokenAsync(userId);
            var cbUrl = $"{callbackBaseUrl}?userId={userId}&code={code}";
            await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + cbUrl + "\">here</a>");
            return true;
        }

        public async Task<ResetPasswordResult> ResetPasswordByEmail(string email, string code, string password)
        {
            var user = await UserManager.FindByNameAsync(email);
            if (user == null)
            {
                return new ResetPasswordResult(){ IsOk = false, ResultStatus = PossibleResultStatus.UserNotFound};
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, code, password);
            if (result.Succeeded)
            {
                return new ResetPasswordResult() { IsOk = true, ResultStatus = PossibleResultStatus.ItsOk, IdentityResult = result};
            }
            return new ResetPasswordResult() { IsOk = false, ResultStatus = PossibleResultStatus.UnknownFailure, IdentityResult = result };
        }

        public async Task<ResetPasswordResult> ResetPasswordByUserId(string userId, string code, string password)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResetPasswordResult() { IsOk = false, ResultStatus = PossibleResultStatus.UserNotFound };
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, code, password);
            if (result.Succeeded)
            {
                return new ResetPasswordResult() { IsOk = true, ResultStatus = PossibleResultStatus.ItsOk, IdentityResult = result };
            }
            return new ResetPasswordResult() { IsOk = false, ResultStatus = PossibleResultStatus.UnknownFailure, IdentityResult = result };
        }

        public async Task<IList<string>> GetValidTwoFactorProviders()
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return null;
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            return userFactors;
        }

        public async Task<bool> SendTwoFactorCode(string provider)
        {
            return await SignInManager.SendTwoFactorCodeAsync(provider);
        }

        public async Task<ExternalLoginResult> ExternalLogin()
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return new ExternalLoginResult{ HasResult = false, IsOk = false};
            }

            // Sign in the user with this external login provider if the user already has a login
            var signInStatus =  await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            return new ExternalLoginResult() { IsOk = true, HasResult = true, LoginInfo = loginInfo, SignInStatus = signInStatus};
        }

        public async Task<ExternalLoginConfirmResult> ExternalLoginConfirm(string username, string email)
        {
            var info = await _authenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return new ExternalLoginConfirmResult(){ IsOk = false, Status = ExternalLoginStatus.ExternalLoginFailure};
            }
            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return new ExternalLoginConfirmResult() { IsOk = true, Status = ExternalLoginStatus.AllDone, HasResult = true, AddLoginResult =  result};
                }
                return new ExternalLoginConfirmResult() { IsOk = false, Status = ExternalLoginStatus.AddLoginFailure };
            }
            return new ExternalLoginConfirmResult(){IsOk = false, Status = ExternalLoginStatus.UserCreationFailure};
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    UserManager = null;
                }

                if (SignInManager != null)
                {
                    SignInManager.Dispose();
                    SignInManager = null;
                }

                this._disposing = false;
            }
        }

        public void Dispose()
        {
            Dispose(_disposing);
            GC.SuppressFinalize(this);
        }
    }
}