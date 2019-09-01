using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;

namespace ConsoleVersion.View
{
    class View
    {
        private static string[] _loginMenu = {
            "Sign in", "Sign up", "Exit"
        };

        private static string[] _vocabularyMenu =
        {
            "Create row", "Show vocabulary", "Change row", "Delete row", "Delete vocabulary", "Learn words",
            "Settings", "Exit"
        };

        private static string[] _settingMenu =
        {
            "Show current user", "Change data", "Delete current user", "Return"
        };

        private static string[] _gameMenu =
        {
            "Write transcription", "Tournament table", "Help", "Setting", "Return to main menu"
        };

        private static readonly int EMPTY = 0;

        private static UserContext _userContext = new UserContext();
        private static VocabularyContext _vocabularyContext = new VocabularyContext();

        public static void LoginMenu()
        {
            do
            {
                UsefulFunctions.PrepareForView();
                byte chooseParagraph = UsefulFunctions.Choose(_loginMenu);

                if (chooseParagraph == 1)
                {
                    User currentUser;
                    do
                    {
                        UsefulFunctions.PrepareForView();
                        currentUser = new User();

                        Console.Write("Nickname: ");
                        currentUser.Nickname = Console.ReadLine();
                        Console.Write("\nPassword: ");
                        currentUser.Password = Console.ReadLine();

                        currentUser = UserController.CompareUser(_userContext, currentUser);
                        if (currentUser == null)
                        {
                            if (Exceptions.IsError != 1)
                                Console.WriteLine("Nickname or password is wrong");
                            else
                                Console.WriteLine(Exceptions.ErrorMessage);
                            
                            Console.ReadKey();
                        }
                        else
                            break;
                    } while (true);

                    OpsMenu(currentUser);
                }
                else if (chooseParagraph == 2)
                {
                    User currentUser = createUser();
                    UserController.AddUser(_userContext, ref currentUser);
                    OpsMenu(currentUser);
                    break;
                }
                else if (chooseParagraph == 3)
                    break;

            } while (true);
        }

        public static void OpsMenu(User currentUser)
        {
            do
            {
                UsefulFunctions.PrepareForView();

                string buff;
                byte chooseOps;

                for (int i = 0; i < _vocabularyMenu.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, _vocabularyMenu[i]);
                }

                buff = Console.ReadLine();
                if (!Byte.TryParse(buff, out chooseOps))
                    continue;

                if (chooseOps == 1)
                {
                    Vocabulary row = createRow(currentUser);
                    VocabularyController.AddRow(_vocabularyContext, row);
                    Console.WriteLine("Row successfully added.");
                    Console.ReadKey();
                }
                else if (chooseOps == 2)
                {
                    Console.WriteLine("Foreign Word\tTranscription\tLocal word");
                    var userVoc = _vocabularyContext.Vocabularies.ToList();
                    foreach (var row in userVoc)
                    {
                        if (row.UserID == currentUser.Id)
                        {
                            if (row.Transcription.Length == 0)
                                Console.WriteLine("{0}\t\t{1}\t\t{2}", row.ForeignWord, "-", row.LocalWord);
                            else
                                Console.WriteLine("{0}\t\t{1}\t\t{2}", row.ForeignWord, row.Transcription, row.LocalWord);
                        }
                    }
                    Console.ReadKey();
                }
                else if (chooseOps == 3)
                {
                    string key;
                    Vocabulary editRow;
                    string text = "Choose the key for find word: \n" +
                                  "1. Foreign word;\n2. Transcription;\n3. Local word";

                    byte editChoose = UsefulFunctions.Choose(text, 1, 3);
                    key = UsefulFunctions.GetKey();

                    try
                    {
                        editRow = VocabularyController.FindRow(key, editChoose, _vocabularyContext);

                        Console.WriteLine("Row successfully found");

                        string change;
                        for (int i = 0; i < 3; i++)
                        {
                            if (i == 0)
                                Console.WriteLine("Edit foreign word. Current is {0}", editRow.ForeignWord);
                            else if (i == 1)
                                Console.WriteLine("Edit transcription. Current is {0}", editRow.Transcription);
                            else
                                Console.WriteLine("Edit local word. Current is {0}", editRow.LocalWord);

                            change = Console.ReadLine();

                            if (change.Length != 0 && i == 0)
                                editRow.ForeignWord = change;
                            else if (change.Length != 0 && i == 1)
                                editRow.Transcription = change;
                            else if (change.Length != 0 && i == 2)
                                editRow.LocalWord = change;
                        }

                        VocabularyController.UpdateRow(_vocabularyContext, editRow);
                    }
                    catch (Exception e)
                    {
                        Exceptions.Catching(e);
                    }

                    if (Exceptions.IsError == EMPTY)
                        Console.WriteLine("Row successfully updated");
                }
                else if (chooseOps == 4)
                {
                    string text = "Choose the key for delete word: \n" +
                                  "1. Foreign word;\n2. Transcription;\n3. Local word";
                    byte deleteChoose = UsefulFunctions.Choose(text, 1, 3);

                    string key = UsefulFunctions.GetKey();
                    try
                    {
                        Vocabulary foundRow = VocabularyController.FindRow(key, deleteChoose, _vocabularyContext);

                        VocabularyController.RemoveRow(_vocabularyContext, foundRow);
                    }
                    catch (Exception e)
                    {
                        Exceptions.Catching(e);
                    }
                    if (Exceptions.IsError == EMPTY)
                        Console.WriteLine("Row successfully deleted");
                }
                else if (chooseOps == 5)
                {
                    //bool sure = false;
                    Console.WriteLine("Are you sure to delete the vocabulary?\n y - yes, n - no");
                    var chooseDelete = Console.ReadLine();
                    if (chooseDelete.Length == 1 && chooseDelete == "n")
                        VocabularyController.RemoveVocabulary(_vocabularyContext, currentUser);

                }
                else if(chooseOps == 6)
                {
                    GameMenu(currentUser);
                }
                else if (chooseOps == 7)
                {
                    currentUser = SettingMenu(currentUser);
                    if (currentUser == null)
                        break;
                }
                else if (chooseOps == 8)
                {
                    break;
                }
            } while (true);
        }

        static User SettingMenu(User currentUser)
        {
            bool exit = false;
            do
            {
                Console.Clear();

                byte settingChoose = UsefulFunctions.Choose(_settingMenu);

                switch (settingChoose)
                {
                    case 1:
                        {
                            Console.WriteLine("Nickname: {0}\nEmail:{1}", currentUser.Nickname, currentUser.Email);
                        }
                        break;
                    case 2:
                        {
                            User checkPassword = new User();
                            checkPassword.Nickname = currentUser.Nickname;
                            Console.Write("Enter the password: ");
                            checkPassword.Password = Console.ReadLine();
                            if (UserController.CompareUser(_userContext, checkPassword) == null)
                                continue;
                            //TO-DO: ask user about what it would like change 
                        }
                        break;
                    case 3:
                        {
                            User checkPassword = new User();
                            checkPassword.Nickname = currentUser.Nickname;
                            Console.Write("Enter the password: ");
                            checkPassword.Password = Console.ReadLine();
                            if (UserController.CompareUser(_userContext, checkPassword) == null)
                                continue;
                            UserController.RemoveUser(_userContext, _vocabularyContext, currentUser);
                            currentUser = null;
                        }
                        break;
                    case 4:
                        {
                            exit = true;
                        }
                        break;

                }

                if (currentUser == null)
                    exit = true;
            } while (!exit);

            return currentUser;
        }

        static void GameMenu(User currentUser)
        {
            UsefulFunctions.PrepareForView();
            GameController gameController = new GameController();
            
            int currentPoints = 0;
            byte gameChoose = UsefulFunctions.Choose(_gameMenu);
            SettingGame settingGame = new SettingGame(_vocabularyContext);

            switch(gameChoose)
            {
                case 1:
                {
                    if (_vocabularyContext.Vocabularies.ToList().Count < GameController.MIN_COUNT)
                        Console.WriteLine("Count of words less than {0}", GameController.MIN_COUNT);
                    else
                    {
                            TimerCallback tm = new TimerCallback(gameController.TimerOver);
                            Timer timer = new Timer(tm, null, GameController.TIMER_MILISECONDS, -1);

                            while (!gameController.isTimerOver)
                            {
                                UsefulFunctions.PrepareForView();
                                int translation = GameController.FOREIGN_TRANSLATION;
                                Vocabulary getRow = gameController.ChooseRandomRow(
                                    _vocabularyContext.Vocabularies.ToList(), ref translation);

                                string word;

                                _ = translation == GameController.FOREIGN_TRANSLATION ?
                                    word = getRow.ForeignWord : (word = getRow.LocalWord);
                                Console.WriteLine("Write the translation to {0}", word);

                                string result = Console.ReadLine();

                                if (gameController.Checker(getRow, result, translation))
                                    currentPoints += settingGame.countPoints;
                            }

                            Console.WriteLine("Count of points is {0}", currentPoints);
                            GameController.WriteToTable(currentPoints, currentUser);
                            Console.ReadLine();
                    }
                }break;
                case 2:
                {

                }break;
                case 4:
                {

                }break;
                case 5:
                    break;
                default:
                    break;
            }
        }



        public static User createUser()
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

        public string EnterTranslate(string word)
        {
            string data;
            Console.Write("Write the translate for {0}: ", word);
            data = Console.ReadLine();
            return data;
        }

        public static Vocabulary createRow(User currentUser)
        {
            Vocabulary row = new Vocabulary();
            string foreignWord, localWord, transcription;
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

            row.UserID = currentUser.Id;
            row.ForeignWord = foreignWord;
            row.Transcription = transcription;
            row.LocalWord = localWord;

            return row;
        }

    }
}

