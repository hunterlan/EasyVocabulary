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

namespace DesktopVersion.View.Game
{
    /// <summary>
    /// Логика взаимодействия для MatchingWords.xaml
    /// </summary>
    public partial class MatchingWords : Window
    {
        Button[] firstGroup, secondGroup;
        private readonly int SIZE_GROUPS = 5;
        public MatchingWords()
        {
            InitializeComponent();
            firstGroup = secondGroup = new Button[SIZE_GROUPS];
        }

        private void AttachButtons(Button[] group, int startWidth, int startHeight = 30)
        {
            for (int i = 0; i < group.Length; i++)
            {
                //Write here the margin
                startHeight += 20;
            }
        }
    }
}
