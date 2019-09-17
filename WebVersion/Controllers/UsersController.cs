using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebVersion.Controllers
{
    public class UsersController : Controller
    {
        //TODO: Write template for pages
        // GET: User
        private UserContext _userContext;
        public UsersController()
        {
            _userContext = new UserContext();
        }
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(User sendedUser)
        {
            User findedUser = UserController.CompareUser(_userContext, sendedUser);
            bool isAuto = false;
            List<User> users = _userContext.Users.ToList();
            foreach (var user in users)
            {
                if (user.Nickname == sendedUser.Nickname)
                {
                    sendedUser.Id = user.Id;
                    isAuto = true;
                }
            }
            if (isAuto)
            {
                Session["UserID"] = sendedUser.Id.ToString();
                return RedirectToAction("Index", "Vocabularies");
            }
            else
                return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
    }
}