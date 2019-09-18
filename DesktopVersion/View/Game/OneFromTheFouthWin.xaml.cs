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
        User currentUser;
        GameController gameController;
        SettingGame settingGame;
        
        int currentPoints;
        string rightAnswer;
        int translation;
        TimerCallback tm;
        Timer timer;
        Button[] buttons;
        public OneFromTheFouthWin(VocabularyContext vocabularyContext, User user)
        {
            InitializeComponent();

            currentUser = user;
            _vocabularyContext = vocabularyContext;
            settingGame = new SettingGame(_vocabularyContext);
            gameController = new GameController();
            tm = new TimerCallback(gameController.TimerOver);
            currentPoints = 0;
            buttons = InittializeButton();
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
                                    _vocabularyContext.Vocabularies.ToList(), ref translation);
            _ = translation == GameController.FOREIGN_TRANSLATION ?
                    word = currentRow.ForeignWord : (word = currentRow.LocalWord);
            if(translation == GameController.FOREIGN_TRANSLATION)
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
            int elemID = rand.Next(buttons.Length);
            for(int i = 0; i < buttons.Length; i++)
            {
                if (i == elemID)
                    buttons[i].Content = rightAnswer;
                else
                {
                    for(int j = 0; j < buttons.Length * 4; j++)
                    {
                        currentRow = gameController.ChooseRandomRow(_vocabularyContext.Vocabularies.ToList());
                        if (translation == GameController.FOREIGN_TRANSLATION)
                            buttons[i].Content = currentRow.LocalWord;
                        else
                            buttons[i].Content = currentRow.ForeignWord;
                    }
                }
            }
        }

        private void ChoosedAnswer(object sender, RoutedEventArgs e)
        {
            Button btnClicker = (Button)sender;
            if ((string)btnClicker.Content == rightAnswer)
                currentPoints += 10;
            if (gameController.isTimerOver)
            {
                MessageBox.Show("Count of points is " + currentPoints);
                Close();
            }
            LoadWord();
        }
    }
}
