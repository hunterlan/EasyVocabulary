﻿using ConsoleVersion.Controllers;
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
            User valueSession = (User)Session["User"];
            if (valueSession == null)
                throw new Exception("You're not authorized!");

            return View();
        }

        [HttpPost]
        public ActionResult GoToAccountPanel(User user)
        {
            UserController.UpdateUser(_userContext, user);
            return RedirectToAction("Index", "Vocabularies");
        }

        public ActionResult Restore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Restore(String email)
        {
            var users = _userContext.Users.ToList();
            User accountUser = null;

            foreach (var item in users)
            {
                if (item.Email == email)
                {
                    accountUser = new User();
                    accountUser.Id = item.Id;
                    accountUser.Nickname = item.Nickname;
                    accountUser.Password = UserController.GeneratePassword();
                    accountUser.Email = email;
                    break;
                }
            }

            if (accountUser != null)
            {
                bool result = UserController.RestoreUser(accountUser, _userContext);

                if (result == false)
                {
                    Exceptions.IsError = 0;
                    throw new Exception(Exceptions.ErrorMessage);
                }
                else
                {
                    Response.Write("<script>alert('Check your email');</script>");
                }

            }
            else
            {
                throw new Exception("User doesn't exist accoring this email!");
            }


            return RedirectToAction("Index", "Start");
        }
    }
}