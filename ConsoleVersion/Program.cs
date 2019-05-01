using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion
{
    internal class Program
    {
        private static UserContext _userContext = new UserContext();
        private static VocabularyContext _vocabularyContext = new VocabularyContext();
        public static void Main(string[] args)
        {
            //User user = createUser();
            //User user = new User {Nickname = "hunterlan", Password = "hunterlan", Email = "hunterlan@gmail.com"};

            //User user = _userContext.Users.FirstOrDefault(user => user.Nickname == "hunterlan");

            var user = _userContext.Users
                .FirstOrDefault(u => u.Nickname == "hunterlan");
            
            Vocabulary vocabulary = new Vocabulary
            {
                ForeignWord = "hello", 
                Transcription = "",
                LocalWord = "привет",
                UserID = user.Id
            };
            //Operations.AddUser(_userContext, user);
            _vocabularyContext.Vocabularies.Add(vocabulary);
            _vocabularyContext.SaveChanges();
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