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
        public string ForeignWord { get; set; }

        public string Transcription { get; set; }

        public string LocalWord { get; set; }
    }
    public partial class Table : Window
    {
        User currentUser;
        private static VocabularyContext _vocabularyContext;
        //TODO: Убрать ID и UserID
        //TODO: Фиксировать изменения в таблице
        public Table(User sessionUser)
        {
            InitializeComponent();
            currentUser = sessionUser;
            _vocabularyContext = new VocabularyContext();
            foreach (Vocabulary row in _vocabularyContext.Vocabularies.ToList())
            {
                VocabularyView.ItemsSource = row.ForeignWord;
                VocabularyView.ItemsSource += row.Transcription;
                VocabularyView.ItemsSource += row.LocalWord;
                break;
            }


        }
    }
}
