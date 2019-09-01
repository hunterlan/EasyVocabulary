using ConsoleVersion.Controllers;
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
    /// Логика взаимодействия для Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        private User currentUser;
        private UserContext _userContext;
        public Setting(User user, UserContext context)
        {
            InitializeComponent();
            currentUser = user;
            _userContext = context;
            ShowUserData();
        }

        private void ShowUserData()
        {
            Nickname.Text = currentUser.Nickname;
            Email.Text = currentUser.Email;
        }

        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            User updatedUser = new User
            {
                Id = currentUser.Id,
                Nickname = Nickname.Text,
                Email = Email.Text,
                Password = currentUser.Password
            };

            if (NewPassword.Password == ConfirmPassword.Password)
                updatedUser.Password = ConfirmPassword.Password;
            updatedUser.Password = SecurePasswordHasher.Hash(updatedUser.Password);
            UserController.UpdateUser(_userContext, updatedUser);
            ShowUserData();
        }
    }
}
