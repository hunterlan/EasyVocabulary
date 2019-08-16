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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public User user;
        public Registration()
        {
            InitializeComponent();
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            User newUser = new User();
            if(NicknameBox.Text.Length == 0 || PasswordBox.Password.Length == 0 ||
                ConfrimBox.Password.Length == 0 || EmailBox.Text.Length == 0)
            {
                MessageBox.Show("All fields are required!");
            }
            else
            {
                if (PasswordBox.Password != ConfrimBox.Password)
                    MessageBox.Show("Passwords are different!");
                else
                {

                    newUser.Nickname = NicknameBox.Text;
                    newUser.Password = PasswordBox.Password;
                    newUser.Email = EmailBox.Text;

                    user = newUser;
                    DialogResult = true;
                }
            }
            

        }
    }
}
