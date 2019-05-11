using System;

namespace ConsoleVersion.Helper
{
    public static class Exceptions
    {
        public static int IsError { get; set; }

        public static void Catching(Exception e)
        {
            Console.WriteLine(e.Message);
            IsError = 1;
        }
    }
}