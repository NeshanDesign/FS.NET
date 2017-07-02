using System.Collections.Generic;
using System.Threading.Tasks;
using FsNet.Data.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using FsNet.Core.Service.Bootstrapping;
using FsNet.Data.Domain.User;
using Microsoft.AspNet.Identity.Owin;

namespace FsNet.Core.Service.AccountService
{
    public interface IUserManagementService: IDisposable
    {
         Guid InstanceId { get; }
        ApplicationSignInManager SignInManager { get; }
        ApplicationUserManager UserManager { get; }
        Task<SignInStatus> Login(string username, string password, bool rememberMe = false, bool shouldLockout = false);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        ApplicationUser GetUserById(string userId);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<bool> GetTwoFactorEnabledAsync(string userId);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<bool> TwoFactorBrowserRememberedAsync(string userId);
        Task<UserViewModel> GetUserInfo(string userId);
        Task<ManageMessageId> RemoveLogin(string userId, string loginProvider, string providerKey);
        Task AddPhoneNumber(string userId, string phoneNumber);
        Task ToggleTwoFactorAuthenticationAsync(string userId, bool enabled);
        Task<IdentityResult> SetPassword(string userId, string newPassword);
        Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber);
        Task<IdentityResult> VerifyPhoneNumber(string userId, string phoneNumber, string token);
        Task<bool> IsVerlfied();
        Task<ManageMessageId> RemovePhoneNumber(string userId);
        Task<SignInStatus> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);
        Task<ChangePasswordResultModel> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<ManageLoginsResult> ManageLogins(string userId);
        Task<ManageMessageId> LinkLoginCallback(string userId, string xsrfKey);

        Task<IdentityResult> RegisterAndSignIn(string username, string email, string password);

        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<bool> SendEmailForForgotPassword(string userId, string callbackBaseUrl);
        Task<ResetPasswordResult> ResetPasswordByEmail(string email, string code, string password);
        Task<ResetPasswordResult> ResetPasswordByUserId(string userId, string code, string password);
        Task<IList<string>> GetValidTwoFactorProviders();
        Task<bool> SendTwoFactorCode(string provider);
        Task<ExternalLoginResult> ExternalLogin();
        Task<ExternalLoginConfirmResult> ExternalLoginConfirm(string username, string email);

        // Task<IdentityResult> AccessFailedAsync(string userId);
        //Task<IdentityResult> AddClaimAsync(string userId, Claim claim);
        //Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        //Task<IdentityResult> AddPasswordAsync(string userId, string password);
        /*  Task<IdentityResult> AddToRoleAsync(string userId, string role);
          Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles);
          Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
          Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token);
          Task<bool> CheckPasswordAsync(string user, string password);
          Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
          Task<IdentityResult> CreateAsync(string user, string password);
          Task<IdentityResult> CreateAsync(string user);
          Task<ClaimsIdentity> CreateIdentityAsync(string user, string authenticationType);
          Task<IdentityResult> DeleteAsync(string user);
          */
    }
}
