using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FsNet.Core.Service.AccountService;
using FsNet.Data.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FsNet.Core.Service.Bootstrapping;

namespace FsNet.UI.User.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private IUserManagementService userService;

        public ManageController()
        {
        }

        public ManageController(IUserManagementService userService)
        {
            userService = this.userService;
        }
       
        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = await userService.GetUserInfo(userId);
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var userId = User.Identity.GetUserId();
            var message = await userService.RemoveLogin(userId, loginProvider, providerKey);
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            await userService.AddPhoneNumber(userId, model.Number);//generate a token an send it to the number
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var userId = User.Identity.GetUserId();
            await userService.ToggleTwoFactorAuthenticationAsync(userId, true);
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var userId = User.Identity.GetUserId();
            await userService.ToggleTwoFactorAuthenticationAsync(userId, false);
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var userId = User.Identity.GetUserId();
            var code = await userService.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var result = await userService.VerifyPhoneNumber(userId, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            AddErrors(result);

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var userId = User.Identity.GetUserId();
            var result = await userService.RemovePhoneNumber(userId);
            return RedirectToAction("Index", result == ManageMessageId.Error ? 
                new { Message = ManageMessageId.Error } : 
                new { Message = result });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var result = await userService.ChangePassword(userId, model.OldPassword, model.NewPassword);
            if (result.IsSucceeded)
            {
                return RedirectToAction("Index", new { Message = result.MessageId });
            }
            AddErrors(result.Result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View(new SetPasswordViewModel());
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var result = await userService.SetPassword(userId, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var result = await userService.ManageLogins(userId);
            if (!result.IsOk)
            {
                return View(result.GeneralMessage);
            }
            
            ViewBag.ShowRemoveButton = result.HasResult;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = result.Logins,
                OtherLogins = result.ExternalLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var userId = User.Identity.GetUserId();
            var result = await userService.LinkLoginCallback(userId, XsrfKey);
            return result == ManageMessageId.Error ? 
                RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error }) : 
                RedirectToAction("ManageLogins");
        }


        #region disposable
        protected override void Dispose(bool disposing)
        {
            if (disposing && userService != null)
            {
                userService.Dispose();
                userService = null;
            }

            base.Dispose(disposing);
        }
        #endregion disposable

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var userId = User.Identity.GetUserId();
            var user =  userService.GetUserById(userId);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var userId = User.Identity.GetUserId();
            var user = userService.GetUserById(userId);
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        

#endregion
    }
}