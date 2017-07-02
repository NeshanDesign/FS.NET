using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FsNet.Data.Domain.ViewModels
{
    public class ExternalLoginResult :BaseResult
    {
        public SignInStatus SignInStatus { get; set; }
        public ExternalLoginInfo LoginInfo { get; set; }
    }

    public class ExternalLoginConfirmResult : BaseResult
    {
        public ExternalLoginStatus Status { get; set; }
        public IdentityResult AddLoginResult { get; set; }
    }

    public class ExternalLoginStatus
    {
        public static ExternalLoginStatus ExternalLoginFailure { get; set; }
        public static ExternalLoginStatus UserCreationFailure { get; set; }
        public static ExternalLoginStatus AddLoginFailure { get; set; }
        public static ExternalLoginStatus AllDone { get; set; }
    }
}
