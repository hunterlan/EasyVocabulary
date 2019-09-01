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
    /// Логика взаимодействия для WriteTransWin.xaml
    /// </summary>
    public partial class WriteTransWin : Window
    {
        VocabularyContext _vocabularyContext;
        User currentUser;
        GameController gameController;
        SettingGame settingGame;
        Vocabulary currentRow;
        int currentPoints;
        int translation;
        TimerCallback tm;
        Timer timer;

        public WriteTransWin(VocabularyContext vocabularyContext, User user)
        {
            InitializeComponent();

            currentUser = user;
            _vocabularyContext = vocabularyContext;
            settingGame = new SettingGame(_vocabularyContext);
            gameController = new GameController();
            tm = new TimerCallback(gameController.TimerOver);
            currentPoints = 0;

            StartGame();
        }

        public void StartGame()
        {
            LoadWord();
            timer = new Timer(tm, null, GameController.TIMER_MILISECONDS, -1);
        }

        public void LoadWord()
        {
            Translator.Text = "";

            translation = GameController.FOREIGN_TRANSLATION;
            string word;

            currentRow = gameController.ChooseRandomRow(
                                    _vocabularyContext.Vocabularies.ToList(), ref translation);
            _ = translation == GameController.FOREIGN_TRANSLATION ?
                word = currentRow.ForeignWord : (word = currentRow.LocalWord);

            ToTranslate.Text = "Write translation to this: " + word;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string result = Translator.Text;

            if (gameController.Checker(currentRow, result, translation))
                currentPoints += settingGame.countPoints;

            if (gameController.isTimerOver)
            {
                MessageBox.Show("Count of points is " + currentPoints);
                GameController.WriteToTable(currentPoints, currentUser);
                Close();
            }
            else
                LoadWord();
        }
    }
}
