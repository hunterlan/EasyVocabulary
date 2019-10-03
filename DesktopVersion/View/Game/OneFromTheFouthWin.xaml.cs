using ConsoleVersion.Controllers;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopVersion
{
    /// <summary>
    /// Логика взаимодействия для OneFromTheFouthWin.xaml
    /// </summary>
    public partial class OneFromTheFouthWin : Window
    {
        VocabularyContext _vocabularyContext;
        List<Vocabulary> words;
        User currentUser;
        SettingGame settingGame;
        GameController gameController;

        int currentPoints;
        string rightAnswer;
        int translation;
        TimerCallback tm;
        Timer timer;
        Button[] buttons;
        private List<Vocabulary> UserVocabulary(List<Vocabulary> voc)
        {
            List<Vocabulary> result = new List<Vocabulary>();

            foreach(var row in voc)
            {
                if (row.UserID == currentUser.Id)
                    result.Add(row);
            }

            return result;
        }
        public OneFromTheFouthWin(VocabularyContext vocabularyContext, User user)
        {
            InitializeComponent();

            currentUser = user;
            _vocabularyContext = vocabularyContext;
            words = UserVocabulary(_vocabularyContext.Vocabularies.ToList());
            //settingGame = new SettingGame(_vocabularyContext);
            gameController = new GameController();
            tm = new TimerCallback(gameController.TimerOver);
            currentPoints = 0;
            buttons = InittializeButton();
            settingGame = new SettingGame(_vocabularyContext);

            StartGame();
        }

        private Button[] InittializeButton()
        {
            Button[] arrive = new Button[4];

            arrive[0] = First; arrive[1] = Second;
            arrive[2] = Third; arrive[3] = Fourth;

            foreach (var elem in arrive)
                elem.Click += ChoosedAnswer;

            return arrive;
        }

        private void StartGame()
        {
            LoadWord();
            timer = new Timer(tm, null, GameController.TIMER_MILISECONDS, -1);
        }

        private void LoadWord()
        {
            TranslateBox.Text = "";

            translation = GameController.FOREIGN_TRANSLATION;
            string word;
            Vocabulary currentRow;

            currentRow = gameController.ChooseRandomRow(
                                    words, ref translation);
            _ = translation == GameController.FOREIGN_TRANSLATION ?
                    word = currentRow.ForeignWord : (word = currentRow.LocalWord);
            if (translation == GameController.FOREIGN_TRANSLATION)
            {
                word = currentRow.ForeignWord;
                rightAnswer = currentRow.LocalWord;
            }
            else
            {
                word = currentRow.LocalWord;
                rightAnswer = currentRow.ForeignWord;
            }
            TranslateBox.Text = "Translate to word " + word;

            Random rand = new Random();
            List<Vocabulary> usedRows = new List<Vocabulary>();
            int elemID = rand.Next(buttons.Length);
            usedRows.Add(currentRow);
            for (int i = 0; i < buttons.Length; i++)
            {
                currentRow = gameController.ChooseRandomRow(words);

                for (int j = 0; j < usedRows.Count; j++)
                {
                    if (currentRow == usedRows[j])
                    {
                        currentRow = gameController.ChooseRandomRow(words);
                        j = 0;
                    }
                }

                usedRows.Add(currentRow);

                if (translation == GameController.FOREIGN_TRANSLATION)
                {
                    buttons[i].Content = currentRow.LocalWord;
                }
                else
                {
                    buttons[i].Content = currentRow.ForeignWord;
                }
            }
            buttons[elemID].Content = rightAnswer;
        }

        private void ChoosedAnswer(object sender, RoutedEventArgs e)
        {
            Button btnClicker = (Button)sender;
            const int THIS_GAME = 1;
            if ((string)btnClicker.Content == rightAnswer)
                currentPoints += 10;
            if (gameController.isTimerOver)
            {
                string typeOfGame = settingGame.TYPE_GAME[THIS_GAME];
                MessageBox.Show("Count of points is " + currentPoints);
                GameController.WriteToTable(currentPoints, currentUser, typeOfGame);
                Close();
            }
            LoadWord();
        }
    }
}
