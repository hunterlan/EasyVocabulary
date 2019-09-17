using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            HelpBox.Text = ReadText();
        }

        private string ReadText()
        {
            const string path = "HelpText.txt";
            string text;
            using(StreamReader sr = new StreamReader(path))
            {
                text = sr.ReadToEnd();
            }

            return text;
        }
    }
}
