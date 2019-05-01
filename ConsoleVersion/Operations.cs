using System;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion
{
    public static class Operations
    {
        public static void AddUser(UserContext userContext, User user)
        {
            userContext.Users.Add(user);
            userContext.SaveChanges();
        }
    }
}