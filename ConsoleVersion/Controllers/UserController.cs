﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

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
            if (userContext.Users.First(u => u.Nickname == user.Nickname) != null)
                return true;
            return false;
        }
    }
}
