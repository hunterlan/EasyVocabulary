using System;

namespace ConsoleVersion.Helper
{
    public static class Exceptions
    {
        public static int IsError { get; set; }

        public static void Catching(Exception e)
        {
            if (e.HResult == -2146233087)
                Console.WriteLine("Can't connect to the server.");
            else
                Console.WriteLine(e.Message);
            IsError = 1;
        }
    }
}