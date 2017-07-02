using System.Linq;
using System.Web.Mvc;
using FsNet.Core.Service.AccountService;
using FsNet.Data.Domain.User;
using FsNet.Data.EF.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FsNet.UI.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManagementService _userService;

        public HomeController(IUserManagementService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page."; 

            return View();
        }
    }
}