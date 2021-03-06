﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using EASendMail;

namespace ConsoleVersion.Controllers
{
    public class UserController
    {
        public static User CompareUser(UserContext userContext, User user)
        {
            User result = null;
            try
            {
                result = userContext.Users.First(d => d.Nickname == user.Nickname);

                if (result != null)
                {
                    if (!SecurePasswordHasher.Verify(user.Password, result.Password))
                        result = null;
                }
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }

            return result;
        }
        public static bool AddUser(UserContext userContext, ref User user)
        {
            bool result = false;
            try
            {
                if (FindUser(userContext, user) == false)
                {
                    user.Password = SecurePasswordHasher.Hash(user.Password);
                    user = userContext.Users.Add(user);
                    userContext.SaveChanges();
                    result = true;
                }
                else
                    throw new Exception("Nickname is using.");
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }

            return result;
        }

        public static bool UpdateUser(UserContext userContext, User changedUser)
        {
            bool result = true;
            try
            {
                userContext.Users.AddOrUpdate(changedUser);
                userContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
                result = false;
            }

            return result;
        }

        public static string GeneratePassword()
        {
            string password = "";

            const int COUNT_SYMB = 12;
            for(int i = 0; i < COUNT_SYMB; i++)
            {
                Random rand = new Random();
                char symb = (char)rand.Next('A', 'z');
                password += symb.ToString();
            }

            return password;
        }
        public static void RemoveUser(UserContext userContext, VocabularyContext vocabularyContext, ref User user)
        {
            try
            {
                VocabularyController.RemoveVocabulary(vocabularyContext, user);
                vocabularyContext.SaveChanges();
                userContext.Users.Remove(user);
                userContext.SaveChanges();
                user = null;
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }

        private static bool FindUser(UserContext userContext, User user)
        {
            foreach(var elem in userContext.Users.ToList())
            {
                if (elem.Nickname == user.Nickname)
                    return true;
            }
            return false;
        }

        public static bool IsPasswordSecure(string password)
        {
            Regex passwordCheck = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])");
            if (passwordCheck.IsMatch(password))
                return true;
            return false;
        }

        public static bool RestoreUser(User accountUser, UserContext userContext)
        {
            // Gmail SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.gmail.com");
            // Using 587 port, you can also use 465 port
            oServer.Port = 587;
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
            oServer.User = "EasyTeamHelp@gmail.com";
            oServer.Password = "20002809ksoh"; //TODO: Hide it

            SmtpMail oMail = new SmtpMail("TryIt");
            oMail.From = oServer.User;
            oMail.To = accountUser.Email;
            oMail.Subject = "Restroring account";
            oMail.TextBody = "Your login: " + accountUser.Nickname + "\nYour new password: " + accountUser.Password +
                "\n\nRespectfully, \nEasy Team";

            SmtpClient oSmtp = new SmtpClient();
            try
            {
                oSmtp.SendMail(oServer, oMail);
                accountUser.Password = SecurePasswordHasher.Hash(accountUser.Password);
                UpdateUser(userContext, accountUser);
            }
            catch(Exception ex)
            {
                Exceptions.Catching(ex);
                return false;
            }

            return true;
        }
    }
}
