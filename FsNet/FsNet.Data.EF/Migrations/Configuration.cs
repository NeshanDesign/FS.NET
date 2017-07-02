using System;
using System.Collections.Generic;
using System.Data.Entity;
using FsNet.Data.EF.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FsNet.Data.Domain.User;
using Microsoft.AspNet.Identity;

namespace FsNet.Data.EF.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            roleManager.Create(new ApplicationRole() { Name = "Admin" , CreatedBy = "System"});
            roleManager.Create(new ApplicationRole() { Name = "User" , CreatedBy = "System"});

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var initUser = new ApplicationUser()
            {
                Email = "passion121@gmail.com",
                PhoneNumber = "989356664770",
                MobileNumber = "989356664770",
                UserName = "admin",
                CreatedBy = "System",
                EmailConfirmed = true,
                CountryPrefix = "IR",
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TwoFactorEnabled = false,
                TelegramUrl = "t.me/NeshanDesign",
                InstagramId = "NeshanDesign"
            };
            userManager.Create(initUser, "p@ssw0rd");
            var user = userManager.FindByEmail("passion121@gmail.com");
            userManager.AddToRoles(user.Id, "Admin", "User");
        }
    }
}
