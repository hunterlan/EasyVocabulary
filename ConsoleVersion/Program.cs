using System;
using System.Diagnostics.PerformanceData;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion
{
    internal class Program
    {
        private static string[] _loginMenu = {
            "Sign in", "Sign up", "Exit"
        };

        private static string[] _vocabularyMenu =
        {
            "Create row", "Show vocabulary", "Change row", "Delete row", "Delete vocabulary", "Exit"
        };
        
        private static UserContext _userContext = new UserContext();
        private static VocabularyContext _vocabularyContext = new VocabularyContext();
        public static void Main(string[] args)
        {
            Menu();
        }

        public static void Menu()
        {
            do
            {
                Console.Clear();
                string buff; 
                byte choose = 0;
                
                for (int i = 0; i < _loginMenu.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i+1, _loginMenu[i]);
                }

                buff = Console.ReadLine();
                if (!Byte.TryParse(buff, out choose))
                    continue;

                if (choose == 1)
                {
                    User currentUser;
                    do
                    {
                        Console.Clear();
                        currentUser = new User();
                        Console.Write("Nickname: ");
                        currentUser.Nickname = Console.ReadLine();
                        Console.Write("\nPassword: ");
                        currentUser.Password = Console.ReadLine();

                        currentUser = Operations.Compare(_userContext, currentUser);
                        if (currentUser == null)
                            Console.WriteLine("Nickname or password is wrong");
                        else
                            break;
                    } while (true);

                    OpsMenu(currentUser);
                }
                else if (choose == 2)
                {
                    User currentUser = createUser();
                    Operations.AddUser(_userContext, currentUser);
                    OpsMenu(currentUser);
                    break;
                }
                else if(choose == 3)
                    break;
                
            } while (true);
        }

        public static void OpsMenu(User currentUser)
        {
            do
            {
                Console.Clear();
                
                string buff; 
                byte choose = 0;
                
                for (int i = 0; i < _vocabularyMenu.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i+1, _vocabularyMenu[i]);
                }

                buff = Console.ReadLine();
                if (!Byte.TryParse(buff, out choose))
                    continue;
                if (choose == 1)
                {
                    
                }
                else if (choose == 2)
                {
                    
                }
                else if(choose == 3)
                {
                    
                }
                else if(choose == 4)
                {
                    
                }
                else if(choose == 5)
                {
                    
                }
                else if(choose == 6)
                {
                    break;
                }
            } while (true);
        }
        

        public static User createUser()
        {
            /*username must only consist of either letters, numbers, periods and underscores */
            
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

        public static Vocabulary createRow()
        {
            Vocabulary row = new Vocabulary();
            string foreignWord = null, localWord = null, transcription = null;
            bool go = false;
            do
            {
                Console.WriteLine("Write foreign word.");
                foreignWord = Console.ReadLine();
                if (foreignWord != null)
                    go = true;
                else
                    Console.WriteLine("Line is empty.");
            } while (!go);
            go = false;
            
            Console.WriteLine("Write transcription (optionally)");
            transcription = Console.ReadLine();

            do
            {
                Console.WriteLine("Write local word.");
                localWord = Console.ReadLine();
                if (localWord != null)
                    go = true;
                else
                    Console.WriteLine("Line is empty.");
            } while (!go);

            row.ForeignWord = foreignWord;
            row.Transcription = transcription;
            row.LocalWord = localWord;

            return row;
        }
    }
    
    
}