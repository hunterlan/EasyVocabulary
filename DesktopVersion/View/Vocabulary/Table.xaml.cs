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
            //TODO: write all games function for desktop version
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
                    var temp = new VocabularyView {

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
                MessageBox.Show("Строка добавлена успешно!");
            else
                MessageBox.Show("Операция отменена");
            LoadGrid();
        }

        private void TableView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var rows = (List<VocabularyView>)TableView.ItemsSource;

            foreach(var row in rows)
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
            Setting settingWindow = new Setting(currentUser, _userContext);
            settingWindow.ShowDialog();
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
    }
}
