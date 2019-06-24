using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion.Controllers
{
    class UserController
    {
        public static User CreateUser()
        {
            string name, password, email;
            User newUser = new User();
            bool go = false;
            do
            {
                Regex reqName = new Regex(@"^(?!.*__.*)(?!.*\.\..*)[a-z0-9_.]+$");
                Console.WriteLine("Write your nickname.");
                name = Console.ReadLine();
                if (name != null && reqName.IsMatch(name))
                    go = true;
                else
                    Console.WriteLine(
                        "Username must only consist of either letters, numbers, periods and underscores");
            } while (!go);

            newUser.Nickname = name;

            do
            {
                go = false;
                Regex reqPass = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
                Console.WriteLine("Write password");
                password = Console.ReadLine();
                if (password != null && password.Length >= 8 && reqPass.IsMatch(password))
                    go = true;
                else
                    Console.WriteLine(
                        "The string must contain at least 1 letter, 1 numeric, minimum 8 characters");
            } while (!go);

            newUser.Password = password;

            do
            {
                go = false;
                Regex reqEmail = new Regex(@"^.+@[^\.].*\.[a-z]{2,}$");
                Console.WriteLine("Write email");
                email = Console.ReadLine();
                if (email != null && reqEmail.IsMatch(email))
                    go = true;
                else
                    Console.WriteLine("That's not the email!");
            } while (!go);

            newUser.Email = email;
            return newUser;
        }

        public static User InputUser()
        {
            User user = new User();

            Console.Write("Nickname: ");
            user.Nickname = Console.ReadLine();
            Console.Write("\nPassword: ");
            user.Password = Console.ReadLine();

            return user;
        }
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

        public static void UserToFile(VocabularyContext vocabularyContext, User user)
        {

        }
    }
}
