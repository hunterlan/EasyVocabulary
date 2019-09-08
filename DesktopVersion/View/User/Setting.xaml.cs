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
        private VocabularyContext _vocabularyContext;
        public Setting(User user, UserContext context, VocabularyContext vocabularyContext)
        {
            InitializeComponent();
            currentUser = user;
            _userContext = context;
            _vocabularyContext = vocabularyContext;
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
            if(UserController.UpdateUser(_userContext, updatedUser) == true)
                ShowUserData();
            else
            {
                MessageBox.Show(Exceptions.ErrorMessage, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Exceptions.IsError = 0;
            }
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Deleting account", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
                UserController.RemoveUser(_userContext, _vocabularyContext, ref currentUser);
        }
    }
}
