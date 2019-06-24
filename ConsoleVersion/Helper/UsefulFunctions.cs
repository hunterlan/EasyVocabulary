using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helper
{
    class UsefulFunctions
    {
        public static byte Choose(string text, byte min, byte max)
        {
            string buffChoose;
            byte userChoose;
            do
            {
                Console.WriteLine(text);
                buffChoose = Console.ReadLine();
                if (!Byte.TryParse(buffChoose, out userChoose) || userChoose < min || userChoose > max)
                    continue;
                break;
            } while (true);

            return userChoose;
        }

        public static byte Choose(string[] chooseArray)
        {
            string buffChoose;
            byte userChoose;

            do
            {
                for (int i = 0; i < chooseArray.Length; i++)
                    Console.WriteLine((i + 1) + ". " + chooseArray[i]);
                buffChoose = Console.ReadLine();
                if (byte.TryParse(buffChoose, out userChoose) && userChoose > 0 && userChoose <= chooseArray.Length)
                    break;
            } while (true);

            return userChoose;
        }

        public static bool ChooseYesOrNo(string text)
        {
            string userChoose;
            Console.WriteLine(text);
            userChoose = Console.ReadLine();

            if (userChoose != null && userChoose.Length > 0 && (userChoose[0] == 'y' || userChoose[0] == 'Y'))
                return true;
            return false;
        }

        public static string GetKey()
        {
            string key;

            do
            {
                Console.Write("Write the key: ");
                key = Console.ReadLine();
                if (key.Length == 0)
                    continue;
                break;
            } while (true);

            return key;
        }

        public static void PrepareForView()
        {
            Exceptions.IsError = 0;
            Console.Clear();
        }
    }
}
