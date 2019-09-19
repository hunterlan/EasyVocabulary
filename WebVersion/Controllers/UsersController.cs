using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebVersion.Controllers
{
    public class UsersController : Controller
    {
        //TODO: Write template for pages
        // GET: User
        private static UserContext _userContext = new UserContext();
     
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(User sendedUser)
        {
            User findedUser = new User();
            bool isAuto = false;
            List<User> users = _userContext.Users.ToList();
            foreach (var user in users)
            {
                if (user.Nickname == sendedUser.Nickname)
                {
                    findedUser = user;
                    isAuto = true;
                    break;
                }
            }
            if (isAuto)
            {
                
                Session["User"] = findedUser;
                return RedirectToAction("Index", "Vocabularies");
            }
            else
                return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User newUser)
        {
            if (UserController.AddUser(_userContext, ref newUser) == false)
            {
                Exceptions.IsError = 0;
                throw new Exception(Exceptions.ErrorMessage);
            }
            else
            {
                Session["User"] = newUser;
                return RedirectToAction("Index", "Vocabularies");
            }
        }

        public ActionResult GoToAccountPanel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoToAccountPanel(User user)
        {
            UserController.UpdateUser(_userContext, user);
            return RedirectToAction("Index", "Vocabularies");
        }
    }
}