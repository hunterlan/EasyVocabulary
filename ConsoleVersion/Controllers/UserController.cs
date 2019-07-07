using System;
using System.Data.Entity.Migrations;
using System.Linq;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion.Controllers
{
    class UserController
    {
        public static User CompareUser(UserContext userContext, User user)
        {
            User result = null;
            try
            {
                result = userContext.Users.Single(d => d.Nickname == user.Nickname);

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
        public static void AddUser(UserContext userContext, ref User user)
        {
            try
            {
                user.Password = SecurePasswordHasher.Hash(user.Password);
                user = userContext.Users.Add(user);
                userContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }

        public static void UpdateUser(UserContext userContext, User changedUser)
        {
            try
            {
                userContext.Users.AddOrUpdate(changedUser);
                userContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }

        public static void RemoveUser(UserContext userContext, VocabularyContext vocabularyContext, User user)
        {
            try
            {
                VocabularyController.RemoveVocabulary(vocabularyContext, user);
                vocabularyContext.SaveChanges();
                userContext.Users.Remove(user);
                userContext.SaveChanges();
            }
            catch (Exception e)
            {
                Exceptions.Catching(e);
            }
        }
    }
}
