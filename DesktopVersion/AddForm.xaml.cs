using ConsoleVersion;
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
    /// Логика взаимодействия для AddForm.xaml
    /// </summary>
    public partial class AddForm : Window
    {
        VocabularyContext _vocabularyContext;
        public AddForm(VocabularyContext vocabularyContext)
        {
            _vocabularyContext = vocabularyContext;
            InitializeComponent();
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            Vocabulary row = new Vocabulary();
            if(ForeignBox.Text.Length == 0 || LocalBox.Text.Length == 0)
            { }
            else
            {
                row.ForeignWord = ForeignBox.Text;
                row.Transcription = TransBox.Text;
                row.LocalWord = LocalBox.Text;
                VocabularyController.AddRow(_vocabularyContext, row);
                if (Exceptions.IsError == 1)
                {
                    MessageBox.Show(Exceptions.ErrorMessage);
                    Exceptions.IsError = 0;
                }
                else
                    DialogResult = true;
            }
        }
    }
}
