using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsNet.Data.Domain.Identity;
using FsNet.Data.EF.Context;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FsNet.Core.UserManagement
{
    public class UserHandler
    {
        public void CreateUser()
        {
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context));
            //var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, int, ApplicationUserRole>(context));

        }
    }



    public interface IUserHandler
    {
        ApplicationUser CreateUser();
    }
}
