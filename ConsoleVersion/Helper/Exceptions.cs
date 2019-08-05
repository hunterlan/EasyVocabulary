using System;

namespace ConsoleVersion.Helper
{
    public static class Exceptions
    {
        public static int IsError { get; set; }

        public static string ErrorMessage { get; set; }

        public static void Catching(Exception e)
        {
            if (e.HResult == -2146233087)
                ErrorMessage = "Can't connect to the server";
            else
                ErrorMessage = e.Message;
            IsError = 1;
        }
    }
}