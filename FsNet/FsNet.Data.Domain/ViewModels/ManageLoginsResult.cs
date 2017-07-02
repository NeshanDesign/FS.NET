using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace FsNet.Data.Domain.ViewModels
{
    public class ManageLoginsResult : BaseResult
    {
        public IList<UserLoginInfo> Logins { get; set; }
        public IList<AuthenticationDescription> ExternalLogins { get; set; }
    }


    public class ResetPasswordResult : BaseResult
    {
        public IdentityResult IdentityResult { get; set; }
        public PossibleResultStatus ResultStatus { get; set; }
    }


    public class PossibleResultStatus
    {
        public static PossibleResultStatus ItsOk;
        public static PossibleResultStatus UserNotFound;
        public static PossibleResultStatus PasswordMisMatch;
        public static PossibleResultStatus InvalidEmail;
        public static PossibleResultStatus UnknownFailure;
    }
}
