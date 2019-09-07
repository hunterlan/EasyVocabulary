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
    /// Логика взаимодействия для MatchingWindow.xaml
    /// </summary>
    public partial class WIP : Window
    {
        public WIP(DateTime date)
        {
            //TODO: Thinking about animation window?
            InitializeComponent();
            Info.Text = "Work in progress. Realese date: " + 
                date.Day + "." + date.Month + "." + date.Year;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
