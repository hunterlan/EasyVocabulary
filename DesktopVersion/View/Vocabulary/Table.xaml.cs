﻿using ConsoleVersion;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для Table.xaml
    /// </summary>\
    class VocabularyView
    {
        private int ID;
        public string ForeignWord { get; set; }

        public string Transcription { get; set; }

        public string LocalWord { get; set; }

        public void setID(int giveID)
        {
            ID = giveID;
        }

        public int getID()
        {
            return ID;
        }
    }
    public partial class Table : Window
    {
        User currentUser;
        private static VocabularyContext _vocabularyContext;
        private UserContext _userContext;
        public Table(User sessionUser, UserContext userContext)
        {
            InitializeComponent();
            currentUser = sessionUser;
            _vocabularyContext = new VocabularyContext();
            _userContext = userContext;
            LoadGrid();
        }

        private void LoadGrid()
        {
            List<VocabularyView> rows = new List<VocabularyView>();
            foreach (var row in _vocabularyContext.Vocabularies.ToList())
            {
                if (row.UserID == currentUser.Id)
                {
                    var temp = new VocabularyView
                    {

                        ForeignWord = row.ForeignWord,
                        Transcription = row.Transcription,
                        LocalWord = row.LocalWord
                    };
                    temp.setID(row.Id);
                    rows.Add(temp);
                }

            }
            TableView.ItemsSource = rows;
        }

        private void ExitAcc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddForm add = new AddForm(_vocabularyContext, currentUser);
            if (add.ShowDialog() == true)
                MessageBox.Show("Row added successfully!");
            else
                MessageBox.Show("Operation canceled");
            LoadGrid();
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Deleting word", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes)
                {
                    var deletedTemperyRow = (VocabularyView)TableView.SelectedItem;
                    Vocabulary row = new Vocabulary
                    {
                        ForeignWord = deletedTemperyRow.ForeignWord,
                        Transcription = deletedTemperyRow.Transcription,
                        LocalWord = deletedTemperyRow.LocalWord,
                        UserID = currentUser.Id
                    };
                    row = VocabularyController.FindRow(row, _vocabularyContext);
                    VocabularyController.RemoveRow(_vocabularyContext, row);
                    LoadGrid();
                } 
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var rows = (List<VocabularyView>)TableView.ItemsSource;

            foreach (var row in rows)
            {
                Vocabulary updatedRow = new Vocabulary
                {
                    Id = row.getID(),
                    ForeignWord = row.ForeignWord,
                    Transcription = row.Transcription,
                    LocalWord = row.LocalWord,
                    UserID = currentUser.Id
                };
                VocabularyController.UpdateRow(_vocabularyContext, updatedRow);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Setting settingWindow = new Setting(currentUser, _userContext, _vocabularyContext);
            settingWindow.ShowDialog();

            if (settingWindow.currentUser == null)
            {
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }

        }

        private void GameTranslation_Click(object sender, RoutedEventArgs e)
        {
            WriteTransWin window = new WriteTransWin(_vocabularyContext, currentUser);
            window.ShowDialog();
        }

        private void OneFromTheFouth_Click(object sender, RoutedEventArgs e)
        {
            OneFromTheFouthWin window = new OneFromTheFouthWin(_vocabularyContext, currentUser);
            window.ShowDialog();
        }

        private void TournTableShow_Click(object sender, RoutedEventArgs e)
        {
            RecordTableView window = new RecordTableView();
            window.ShowDialog();
        }

        private void Matching_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Make this game function
            DateTime release = new DateTime(2019, 9, 28);
            WIP window = new WIP(release);
            window.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow window = new HelpWindow();
            window.ShowDialog();
        }

    }
}
